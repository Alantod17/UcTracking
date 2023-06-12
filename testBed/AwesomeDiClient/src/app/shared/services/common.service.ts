import { Injectable } from '@angular/core';
import { EventBrokerService } from './event.service';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  constructor(private eventBroker: EventBrokerService, private router: Router, private snackBar: MatSnackBar) { }
  public momentjs = moment;
  public getApiEndPoint() {
    return "https://localhost:5001/";
  }
  public loadLocalData(key: string): any {
    let data = localStorage.getItem(key);
    return data == null ? null : JSON.parse(data);
  }
  public setLocalData(key: string, data: any) {
    localStorage.setItem(key, JSON.stringify(data));
  }
  public removeLocalData(key: string) {
    localStorage.removeItem(key);
  }
  public hasValue(value: any): boolean {
    let res = true;
    if (value == null) res = false;
    if (value === "") res = false;
    if (typeof value === "undefined") res = false;
    return res;
  }
  public logout() {
    this.removeLocalData("currentUser");
    this.eventBroker.emit<any>("di-logout-success", {});
    this.router.navigate(["home/login"]);
  }
  public isValidEmail(email: string) {
    let res = true;
    const expression = /(?!.*\.{2})^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    email = email.replace(" ", "");
    email = email.replace(",", ";");
    let emailList: any[] = [];
    if (email.indexOf(';') > -1) emailList = email.split(';');
    emailList.forEach(val => {
      if (expression.test(String(val).toLowerCase()) == false) res = false;
    })
    return res;
  }
  public getLocalDateString(utcDateString: string): string {
    var stillUtc = moment.utc(utcDateString).toDate();
    var local = moment(stillUtc).local().format('DD/MM/YYYY');
    return local;
  }
  public getUtcDateString(localDateString: string): string {
    var utc = moment(localDateString).utc().format();
    return utc;
  }
  public getNowDateTimeString(): string {
    return moment().format();
  }
  public getNowUtcDateTimeString(): string {
    return moment().utc().format();
  }
  public getUtcDateStringForDate(localDate: Date): string {
    var utc = moment(localDate.toString()).utc().format();
    return utc;
  }
  public getDateBegin(localDate: Date): Date {
    var start = new Date(localDate.toISOString());
    start.setHours(0,0,0,0);
    return start;
  }
  public getDateEnd(localDate: Date): Date {
    var end = new Date(localDate.toISOString());
    end.setHours(23,59,59,999);
    return end;
  }
  public addDays(date: string, daysToAdd: number): string {
    return moment(date).add(daysToAdd, "days").format();
  }
  public sharedStart(array: any[]) {
    var A = array.concat().sort(),
      a1 = A[0], a2 = A[A.length - 1], L = a1.length, i = 0;
    while (i < L && a1.charAt(i) === a2.charAt(i)) i++;
    return a1.substring(0, i);
  }

  public notifyErrs(input: any[]) {
    if (input) {
      var errors = input.map(e => e.value).join("\r\n");
      this.snackBar.open(errors, 'Close', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'bottom',
        panelClass: ['multi-line-snackbar']
      });
    }
  }
  public notifyErr(error: string) {
    this.snackBar.open(error, 'Close', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'bottom',
      panelClass: ['multi-line-snackbar']
    });
  }
  public setCurrentUser(user: any) {
    this.setLocalData("currentUser", user);
  }
  public getCurrentUser(): CurrentUser | null {
    let data = this.loadLocalData("currentUser");
    if (this.hasValue(data)) {
      return new CurrentUser(data);
    }
    return null;
  }

  public newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      let r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  public isVideo(fileExt: string) {
    if (!fileExt) return false;
    let extList = [".MP4", ".MOV", ".WMV", ".AVI", ".MKV", ".MPG", ".M4V"];
    return extList.indexOf(fileExt.toUpperCase()) > -1;
  }

  public isImage(fileExt: string) {
    if (!fileExt) return false;
    let extList = [".JPG", ".JPE", ".BMP", ".GIF", ".PNG", ".JPEG"];
    return extList.indexOf(fileExt.toUpperCase()) > -1;
  }
}

export class CurrentUser {
  constructor(data?: any) {
    if (typeof data !== 'undefined' && data !== null) {
      Object.assign(this, data);
    }
  }
  roles!: string[];
  email!: string;
  accessToken!: string;
  accessTokenExpireUtcDateTime!: string;
  refreshToken!: string;
  refreshTokenExpireUtcDateTime!: string;
}