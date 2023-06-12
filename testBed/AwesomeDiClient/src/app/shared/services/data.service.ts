import { SettingService } from './setting.service';
import { LoginParameter, LoginResult, SearchTagResult, SearchTagParameter, CreateExpenseParameter, CreateExpenseResult, RefreshTokenParameter, RefreshTokenResult, SearchShopParameter, SearchShopResult, SearchExpenseParameter, SearchExpenseResult, SignUpParameter, SignUpResult, SearchFileEntryParameter, SearchFileEntryResult, GoogleLoginParameter, GoogleLoginResult, GetThumbnailImageParameter, GetThumbnailImageResult, GetFileEntryResult, GetFileEntryParameter, SearchFileEntryGroupParameter, SearchFileEntryGroupResult, SearchResearchArticleParameter, SearchResearchArticleResult, GetResearchArticleResult, SearchResearchArticlePagedResult, UpdateResearchArticleStatusParameter, UpdateResearchArticleResearchDetailParameter, GetResearchArticleInfoListResult, SearchSharesiesInstrumentPagedResult, SearchSharesiesInstrumentParameter, SearchTrackingDataResult, SearchTrackingDataParameter, GetTrackingDataParameter, GetTrackingDataResult } from './dto';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CommonService } from './common.service';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  apiEndPoint = '';
  constructor(private http: HttpClient, private commonService: CommonService) {
    this.apiEndPoint = SettingService.settings.apiEndPoint;
  }
  private getHttpParam(param: any): HttpParams {

    return Object.getOwnPropertyNames(param).filter(p => param[p] != null).reduce((p, key) => p.set(key, param[key]), new HttpParams());
  }
  public login(param: LoginParameter): Observable<LoginResult> {
    return this.http.post<LoginResult>(this.apiEndPoint + 'Auth/Login', param);
  }
  public googleLogin(param: GoogleLoginParameter): Observable<GoogleLoginResult> {
    return this.http.post<GoogleLoginResult>(this.apiEndPoint + 'Auth/GoogleLogin', param);
  }
  public signUp(param: SignUpParameter): Observable<SignUpResult> {
    return this.http.post<SignUpResult>(this.apiEndPoint + 'SignUp', param);
  }
  public createExpense(param: CreateExpenseParameter): Observable<CreateExpenseResult> {
    return this.http.post<CreateExpenseResult>(this.apiEndPoint + 'Expense', param);
  }
  public refreshToken(param: RefreshTokenParameter): Observable<RefreshTokenResult> {
    return this.http.post<RefreshTokenResult>(this.apiEndPoint + 'Auth/RefreshToken', param);
  }
  public searchTag(param: SearchTagParameter): Observable<SearchTagResult[]> {
    return this.http.get<SearchTagResult[]>(this.apiEndPoint + 'Tag', { params: this.getHttpParam(param) });
  }
  public searchShop(param: SearchShopParameter): Observable<SearchShopResult[]> {
    return this.http.get<SearchShopResult[]>(this.apiEndPoint + 'Shop', { params: this.getHttpParam(param) });
  }
  public searchExpense(param: SearchExpenseParameter): Observable<SearchExpenseResult[]> {
    return this.http.get<SearchExpenseResult[]>(this.apiEndPoint + 'Expense', { params: this.getHttpParam(param) });
  }
  public deleteExpense(id: number): Observable<any> {
    return this.http.delete(this.apiEndPoint + 'Expense/' + id);
  }
  public markExpenseAsNotDuplicate(id: number): Observable<any> {
    return this.http.put(this.apiEndPoint + 'Expense/' + id + '/NotDuplicate', {});
  }
  public searchFileEntry(param: SearchFileEntryParameter): Observable<SearchFileEntryResult[]> {
    return this.http.get<SearchFileEntryResult[]>(this.apiEndPoint + 'file', { params: this.getHttpParam(param) });
  }
  public searchFileEntryGroup(param: SearchFileEntryGroupParameter): Observable<SearchFileEntryGroupResult[]> {
    return this.http.get<SearchFileEntryGroupResult[]>(this.apiEndPoint + 'fileGroup', { params: this.getHttpParam(param) });
  }
  public getFileEntry(param: GetFileEntryParameter): Observable<GetFileEntryResult> {
    return this.http.get<GetFileEntryResult>(this.apiEndPoint + 'file/Get', { params: this.getHttpParam(param) });
  }
  public getThumbnailImage(param: GetThumbnailImageParameter): Observable<GetThumbnailImageResult> {
    return this.http.get<GetThumbnailImageResult>(this.apiEndPoint + 'file/GetThumbnailImage', { params: this.getHttpParam(param) });
  }
  public searchResearchArticle(param: SearchResearchArticleParameter): Observable<SearchResearchArticleResult[]> {
    return this.http.get<SearchResearchArticleResult[]>(this.apiEndPoint + 'ResearchArticle', { params: this.getHttpParam(param) });
  }
  public searchResearchArticlePaged(param: SearchResearchArticleParameter): Observable<SearchResearchArticlePagedResult> {
    return this.http.get<SearchResearchArticlePagedResult>(this.apiEndPoint + 'ResearchArticle/Paged', { params: this.getHttpParam(param) });
  }
  public getResearchArticle(id: number): Observable<GetResearchArticleResult> {
    return this.http.get<GetResearchArticleResult>(this.apiEndPoint + 'ResearchArticle/' + id + '?reqId=' + this.commonService.newGuid());
  }
  public updateResearchArticleStatus(id: number, param: UpdateResearchArticleStatusParameter): Observable<object> {
    return this.http.post(this.apiEndPoint + 'ResearchArticle/' + id, param);
  }
  public updateResearchArticleResearchDetail(id: number, param: UpdateResearchArticleResearchDetailParameter): Observable<object> {
    return this.http.post(this.apiEndPoint + 'ResearchArticle/' + id + '/ResearchDetail', param);
  }
  public getResearchArticleInfoList(): Observable<GetResearchArticleInfoListResult> {
    return this.http.get<GetResearchArticleInfoListResult>(this.apiEndPoint + 'ResearchArticle/InfoList');
  }
  public searchSharesiesInstrument(param: SearchSharesiesInstrumentParameter): Observable<SearchSharesiesInstrumentPagedResult> {
    return this.http.get<SearchSharesiesInstrumentPagedResult>(this.apiEndPoint + 'SharesiesInstruments', { params: this.getHttpParam(param) });
  }
  public searchTrackingData(param: SearchTrackingDataParameter): Observable<SearchTrackingDataResult[]> {
    console.debug("1111111111",param);
    return this.http.get<SearchTrackingDataResult[]>(this.apiEndPoint + 'Tracking/Search', { params: this.getHttpParam(param) });
  }
  public getTrackingData(param: GetTrackingDataParameter): Observable<GetTrackingDataResult[]> {
    return this.http.get<GetTrackingDataResult[]>(this.apiEndPoint + 'Tracking/Get', { params: this.getHttpParam(param) });
  }
}
