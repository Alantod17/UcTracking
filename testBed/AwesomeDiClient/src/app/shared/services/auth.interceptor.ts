import { EventBrokerService } from './event.service';
import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { Router } from '@angular/router';
import { DataService } from './data.service';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';
import { RefreshTokenParameter } from './dto';
import * as moment from 'moment';
import { genericRetryStrategy } from './rxjs-utils';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError, map, retryWhen, switchMap, tap } from 'rxjs/operators';
@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    refreshTokenInProgress = false;

    tokenRefreshedSource = new Subject();
    tokenRefreshed$ = this.tokenRefreshedSource.asObservable();

    constructor(private router: Router, private api: DataService, private commonService: CommonService, private eventBroker: EventBrokerService) {
    }

    addAuthHeader(request: HttpRequest<any>) {
        const currentUser = this.commonService.getCurrentUser();
        if (currentUser) {
            return request.clone({
                setHeaders: {
                    'Content-Type': 'application/json',
                    Authorization: 'Bearer ' + currentUser.accessToken
                }
            });
        }
        return request;
    }

    refreshToken() {
        if (this.refreshTokenInProgress) {
            return new Observable(observer => {
                this.tokenRefreshed$.subscribe(() => {
                    observer.next();
                    observer.complete();
                });
            });
        } else {
            this.refreshTokenInProgress = true;
            const currentUser = this.commonService.getCurrentUser();
            const param = new RefreshTokenParameter();
            param.accessToken = currentUser?.accessToken ?? '';
            param.refreshToken = currentUser?.refreshToken ?? '';
            return this.api.refreshToken(param)
                .pipe(retryWhen(genericRetryStrategy({
                    maxRetryAttempts: 3,
                    scalingDuration: 500,
                    excludedStatusCodes: []
                })))
                .pipe(map(response => {
                    this.commonService.setCurrentUser(response);
                    return response;
                }))
                .pipe(tap(() => {
                    this.refreshTokenInProgress = false;
                    this.tokenRefreshedSource.next(true);
                }));

        }
    }

    logout() {
        this.commonService.logout();
    }


    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<any> {

        // Handle request
        request = this.addAuthHeader(request);

        // Handle response
        return next.handle(request).pipe(catchError(error => {
            if (error.status === 401) {
                return this.refreshToken()
                    .pipe(switchMap(() => {
                        request = this.addAuthHeader(request);
                        return next.handle(request);
                    }))
                    .pipe(catchError(() => {
                        this.logout();
                        return throwError(error);
                    }));
            } else if (error.status === 403) {
                this.commonService.notifyErr('Access is forbiddened');
                return throwError(error);
            } else if (error.status === 400) {
                if (Array.isArray(error.error)) {
                    this.commonService.notifyErrs(error.error);
                }
                return throwError(error);
            } else {
                if (error.error instanceof ErrorEvent) {
                    // A client-side or network error occurred. Handle it accordingly.
                    console.error('An error occurred:', error.error.message);
                } else {
                    // The backend returned an unsuccessful response code.
                    // The response body may contain clues as to what went wrong, // returns null
                    console.error(
                        `Backend returned code ${error.status}, ` +
                        `body was: ${error.error}`);
                }
                // return an observable with a user-facing error message
                return throwError(error);
            }
        }));
    }
}
