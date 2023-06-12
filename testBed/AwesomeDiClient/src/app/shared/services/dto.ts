import { SafeUrl } from "@angular/platform-browser";

export class Base {
    requestDateTime!: string;
}

export class LoginParameter extends Base {
    email!: string;
    password!: string;
    deviceId: string | undefined | null;
}

export class LoginResult {
    email: string | undefined;
    accessToken: string | undefined;
    accessTokenExpireUtcDateTime: string | undefined;
    refreshToken: string | undefined;
    refreshTokenExpireUtcDateTime: string | undefined;
}


export class GoogleLoginParameter extends Base {
    email!: string;
    idToken!: string;
    deviceId: string | undefined | null;
}

export class GoogleLoginResult {
    email: string | undefined;
    accessToken: string | undefined;
    accessTokenExpireUtcDateTime: string | undefined;
    refreshToken: string | undefined;
    refreshTokenExpireUtcDateTime: string | undefined;
}


export class SignUpParameter extends Base {
    email: string | undefined;
    username: string | undefined;
    confirmPassword: string | undefined;
    password: string | undefined;
}

export class RefreshTokenParameter extends Base {
    accessToken!: string;
    refreshToken!: string;
}

export class RefreshTokenResult {
    email!: string;
    accessToken!: string;
    accessTokenExpireUtcDateTime!: string;
    refreshToken!: string;
    refreshTokenExpireUtcDateTime!: string;
}

export class CreateExpenseParameter extends Base {
    transactionDate!: string;
    amount!: number;
    description!: string;
    mainTag!: string;
    optionalTags!: string;
}

export class CreateExpenseResult {
    oid!: number;
    transactionDate!: string;
    amount!: number;
    description!: string;
}


export class SearchTagParameter extends Base {
    ownerEmail!: string;
    isMainTag!: boolean | null;
}

export class SearchTagResult {
    oid!: number;
    description!: string;
    isMainTag!: boolean;
}

export class SearchShopParameter extends Base {
    ownerEmail!: string;
}

export class SearchShopResult {
    oid!: number;
    description!: string;
    mainTag!: string;
    optionalTags!: string;
}

export class SearchExpenseParameter extends Base {
    startDate!: Date | string | null;
    endDate!: Date | string | null;
    mainTag!: string;
    optionalTags!: string;
    ownerEmail!: string;
}

export class SearchExpenseResult {
    oid!: number;
    transactionDate!: Date | string;
    amount!: number;
    description!: string;
    mainTag!: string;
    optionalTags!: string;
    isDuplicate!: boolean;
}


export class SignUpResult {
    oid!: number;
    username!: string;
    email!: string;
    accessToken!: string;
    rtGuid!: string | null;
}


export class GallaryFile {
    id!: number | null;
    fileBase64!: string | null | SafeUrl;
    thumbnailUrl!: string | null | SafeUrl;
    enteredDate!: string;
    enteredMonth!: string;
    enteredYear!: string;
    extension!: string;
    width!: number;
    height!: number;
}

export class SearchFileEntryParameter {
    take: number | undefined;
    skip: number | undefined;
}

export class SearchFileEntryResult {
    id!: number | null;
    filePath!: string | null;
    lastWriteUtcDateTime!: string | null;
    extension!: string;
    width!: number;
    height!: number;
}

export class GetThumbnailImageParameter {
    id!: number | null;
}

export class GetThumbnailImageResult {
    id!: number;
    fileBase64!: string;
}

export class GetFileEntryParameter {
    id!: number | null;
}

export class GetFileEntryResult {
    id!: number;
    fileHeight!: number;
    fileWidth!: number;
    fileTypeExtension!: string;
    fileBase64!: string;
    extension!: string;
    lastWriteUtcDateTime!: string;
}

export interface SearchFileEntryGroupParameter {
    startUtcDate: string | null;
    endUtcDate: string | null;
    groupBy: string;
}

export interface SearchFileEntryGroupResult {
    groupKey: string;
    fileList: SearchFileEntryGroupResultFile[];
}

export interface SearchFileEntryGroupResultFile {
    id: number;
    width: number | null;
    height: number | null;
    extension: string;
    lastWriteUtcDateTime: string;
}

export interface SearchResearchArticleParameter {
    isDeleted?: boolean | null;
    isDuplicate?: boolean | null;
    isNeedReview?: boolean | null;
    IsIrrelevant?: boolean | null;
    sortField?: string | null;
    sortDirection?: string | null;
    Keywords?: string | null;
    take?: number;
    skip?: number;
}

export interface SearchResearchArticleResult {
    id: number;
    title: string;
    abstract: string;
    year: string;
    isDeleted: boolean;
    isDuplicate: boolean;
    isNeedReview: boolean;
}
export interface SearchResearchArticlePagedResult {
    totalCount: number;
    results: SearchResearchArticleResult[];
}
export interface GetResearchArticleResult {
    id: number;
    EntryType: string;
    EntryKey: string;
    author: string;
    title: string;
    year: string;
    isbn: string;
    publisher: string;
    address: string;
    url: string;
    doi: string;
    abstract: string;
    booktitle: string;
    pages: string;
    numpages: string;
    keywords: string;
    location: string;
    series: string;
    issue_date: string;
    volume: string;
    number: string;
    issn: string;
    journal: string;
    month: string;
    articleno: string;
    isDuplicate: boolean;
    isIrrelevant: boolean;
    notes: string;
    isNeedReview: boolean;
    isDeleted: boolean;
    researchGoal?: string;
    researchQuestions?: string;
    researchMethod?: string;
    researchMethodList: string[];
    researchDataSource?: string;
    researchDataSourceList: string[];
    researchContext?: string;
    researchContextList: string[];
    researchedProblemType?: string;
    researchedProblemTypeList: string[];
    researchPlatform?: string;
    researchPlatformList: string[];
    researchedTechnology?: string;
    researchedTechnologyList: string[];
    researchedApproach?: string;
    researchedApproachList: string[];
    researchMainFindings?: string;
    researchContributionType?: string;
    researchContributionTypeList: string[];
    researchFutureWorks?: string;
    researchChallenges?: string;
    researchBugRootCause?: string;
    researchBugRootCauseList: string[];
    link?: string;

}


export interface UpdateResearchArticleStatusParameter {
    id?: number | null;
    isDuplicate?: boolean;
    isIrrelevant?: boolean;
    isNeedReview?: boolean;
    isDeleted?: boolean;
}

export interface UpdateResearchArticleResearchDetailParameter {
    id?: number | null;
    researchGoal?: string;
    Notes?: string;
    researchQuestions?: string;
    researchMethod?: string;
    researchDataSource?: string;
    researchContext?: string;
    researchedProblemType?: string;
    researchPlatform?: string;
    researchedTechnology?: string;
    researchedApproach?: string;
    researchMainFindings?: string;
    researchContributionType?: string;
    researchFutureWorks?: string;
    researchChallenges?: string;
    researchBugRootCause?: string;
    researchMethodList: string[];
    researchDataSourceList: string[];
    researchContextList: string[];
    researchedProblemTypeList: string[];
    researchPlatformList: string[];
    researchedTechnologyList: string[];
    researchedApproachList: string[];
    researchContributionTypeList: string[];
    researchBugRootCauseList: string[];
}


export interface GetResearchArticleInfoListResult {
    researchMethodList: string[];
    researchDataSourceList: string[];
    researchContextList: string[];
    researchedProblemTypeList: string[];
    researchPlatformList: string[];
    researchedTechnologyList: string[];
    researchedApproachList: string[];
    researchContributionTypeList: string[];
    researchBugRootCauseList: string[];
}

export interface SearchSharesiesInstrumentParameter {
    take: number | null;
    skip: number | null;
    sortField: string| null;
    sortDirection: string| null;
    keywords: string| null;
    exchangeCountry: string| null;
}

export interface SearchSharesiesInstrumentResult {
    id: number;
    name: string;
    exchangeCountry: string | null;
    lastPrice: number | null;
    dayChangePercent: number | null;
    threeDayChangePercent: number | null;
    weekChangePercent: number | null;
    twoWeekChangePercent: number | null;
    monthChangePercent: number | null;
    twoMonthChangePercent: number | null;
    threeMonthChangePercent: number | null;
    sixMonthChangePercent: number | null;
    yearChangePercent: number | null;
    sharesiesId: string;
    urlSlug: string;
    riskRating: number;
}

export interface SearchSharesiesInstrumentPagedResult {
    totalCount: number;
    results: SearchSharesiesInstrumentResult[];
}

export class SearchTrackingDataParameter {
    trackingId: string | undefined;
    startDateTime: string | null | undefined;
    endDateTime: string | null | undefined;
}

export interface SearchTrackingDataResult {
    trackingId: string;
    startDateTime: string;
    endDateTime: string;
}

export class GetTrackingDataParameter {
    trackingId: string | undefined;
    startDateTime: string | null | undefined;
    endDateTime: string | null | undefined;
}

export interface GetTrackingDataResult {
    type: string;
    trackingId: string;
    dateTime: string;
    dataDetail: string;
}