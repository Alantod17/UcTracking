import { RefreshTokenParameter } from './dto';
import { DataService } from './data.service';
import { Injectable } from '@angular/core';
import { Router, CanActivate, CanLoad, ActivatedRouteSnapshot, RouterStateSnapshot, Route, UrlSegment } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CommonService } from './common.service';

@Injectable()
export class AuthGuardService implements CanActivate, CanLoad {
  constructor(public router: Router, private commonService: CommonService, private dataService: DataService) { }
  // canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
  //   return this.isLoggedIn(state.url);
  // }

  canLoad(route: Route, segments: UrlSegment[]): boolean {
    let fullPath = segments.reduce((path, currentSegment) => {
      return `${path}/${currentSegment.path}`;
    }, '');
    return this.isLoggedIn(fullPath);
  }

  private isLoggedIn(targetRoute: string): boolean {
    // let currentUser = this.commonService.loadLocalData('currentUser');
    // if (!currentUser || !currentUser.accessToken) {
    //   this.router.navigate(['login'], { queryParams: { redirectUrl: targetRoute } });
    //   return false;
    // }
    return true;
  }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let currentUser = this.commonService.loadLocalData('currentUser');
    let path = route.routeConfig?.path;
    let token = "";
    if (currentUser) token = currentUser.accessToken;
    const jwtHelper = new JwtHelperService();
    if (route.data && route.data.roles && route.data.roles.indexOf(currentUser.role) === -1) {
      // role not authorised so redirect to home page
      this.router.navigate(["home/login"], { queryParams: { redirectUrl: path } });
      return false;
    }
    if (token && !jwtHelper.isTokenExpired(token)) {
      return true;
    }
    const isRefreshSuccess = await this.tryRefreshingTokens();
    if (!isRefreshSuccess) {
      this.router.navigate(["home/login"], { queryParams: { redirectUrl: path } });
    }
    return isRefreshSuccess;
  }

  private async tryRefreshingTokens(): Promise<boolean> {

    let currentUser = this.commonService.getCurrentUser();
    if (!currentUser || !currentUser.accessToken || !currentUser.refreshToken) {
      return false;
    }

    let isRefreshSuccess: boolean;
    try {
      let param = new RefreshTokenParameter();
      param.accessToken = currentUser.accessToken;
      param.refreshToken = currentUser.refreshToken;
      const response = await this.dataService.refreshToken(param).toPromise();
      this.commonService.setCurrentUser(response);
      isRefreshSuccess = true;
    }
    catch (ex) {
      isRefreshSuccess = false;
    }
    return isRefreshSuccess;
  }
}