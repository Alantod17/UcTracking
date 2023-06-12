import { SafePipe } from './pipes/safe.pipe';
import { DataService } from './services/data.service';
import { CommonService } from './services/common.service';
import { Injectable, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedRoutingModule } from './shared-routing.module';
//Bootstrap
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
//Material
// import { LoginComponent } from './login/login.component';
// import { HomeComponent } from './home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

//Angular Material Components
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';;
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTabsModule } from '@angular/material/tabs';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';


import { HttpClient, HttpClientModule, HttpHeaders, HTTP_INTERCEPTORS } from '@angular/common/http';
import { DiMatFormEmailComponent } from './elements/di-mat-form-email/di-mat-form-email.component';
import { DiMatButtonComponent } from './elements/di-mat-button/di-mat-button.component';
import { MatProgressButtonsModule } from 'mat-progress-buttons';
import { DateStringPipe } from './pipes/date-string.pipe';
import { DiMatDatePickerComponent } from './elements/di-mat-date-picker/di-mat-date-picker.component';
import { DiMatConfirmDialogComponent } from './elements/di-mat-confirm-dialog/di-mat-confirm-dialog/di-mat-confirm-dialog.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DiMatImageDialogComponent } from './elements/di-mat-image-dialog/di-mat-image-dialog.component';
import { SrcStringPipe } from './pipes/src-string.pipe';
import { UiImageLoaderDirective } from './directives/ui-image-loader.directive';
import { Attributes, IntersectionObserverHooks, LazyLoadImageModule, LAZYLOAD_IMAGE_HOOKS } from 'ng-lazyload-image';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthGuardService } from './services/auth-guard.service';
import { AuthInterceptor } from './services/auth.interceptor';
import { NgFloatingActionMenuModule } from 'ng-floating-action-menu';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FileViewerModalComponent } from './elements/file-viewer-modal/file-viewer-modal.component';
import { DiBooleanToggleComponent } from './elements/di-boolean-toggle/di-boolean-toggle.component';
import { DiMatChipComponent } from './elements/di-mat-chip/di-mat-chip.component';
import { DiMatIframeDialogComponent } from './elements/di-mat-iframe-dialog/di-mat-iframe-dialog.component';
import { DiMatDateTimePickerComponent } from './elements/di-mat-date-time-picker/di-mat-date-time-picker.component';
import { NgxMatDateAdapter, NgxMatDateFormats, NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule, NGX_MAT_DATE_FORMATS } from '@angular-material-components/datetime-picker';
import { NGX_MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular-material-components/moment-adapter';
import { CustomNgxDatetimeAdapter } from './elements/di-mat-date-time-picker/CustomNgxDatetimeAdapter';
import { RouteReuseStrategy } from '@angular/router';
import { RouteReuseService } from './services/route-reuse-service';

const CUSTOM_DATE_FORMATS: NgxMatDateFormats = {
  parse: {
    dateInput: 'l, LTS'
  },
  display: {
    dateInput: 'DD/MM/YYYY HH:mm:ss',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  }
};

@Injectable()
export class LazyLoadImageHooks extends IntersectionObserverHooks {
  private http: HttpClient;

  constructor(http: HttpClient, private commonService: CommonService) {
    super();
    this.http = http;
  }

  loadImage({ imagePath }: Attributes): Observable<string> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.commonService.getCurrentUser()?.accessToken}`,
      'Content-Type': 'application/json',
    })
    return this.http.get(imagePath, { responseType: 'blob', headers: headers }).pipe(map(blob => URL.createObjectURL(blob)));
  }
}

@NgModule({
  declarations: [
    // LoginComponent,
    DiMatFormEmailComponent,
    DiMatButtonComponent,
    DateStringPipe,
    SafePipe,
    SrcStringPipe,
    DiMatDatePickerComponent,
    DiMatConfirmDialogComponent,
    DiMatIframeDialogComponent,
    DiMatImageDialogComponent,
    UiImageLoaderDirective,
    FileViewerModalComponent,
    DiBooleanToggleComponent,
    DiMatChipComponent,
    DiMatIframeDialogComponent,
    DiMatDateTimePickerComponent,
  ],
  entryComponents: [
    DiMatConfirmDialogComponent, 
    DiMatIframeDialogComponent, 
    DiMatImageDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    SharedRoutingModule,
    FontAwesomeModule,
    LazyLoadImageModule,
    NgFloatingActionMenuModule,
    //ngx-Bootstrap
    BsDropdownModule.forRoot(),
    CollapseModule.forRoot(),
    ModalModule.forRoot(),
    //Material 
    MatCheckboxModule,
    MatCheckboxModule,
    MatButtonModule,
    MatInputModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatStepperModule,
    MatTabsModule,
    MatExpansionModule,
    MatButtonToggleModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatDialogModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatProgressButtonsModule,
    MatNativeDateModule,
    NgxMatDatetimePickerModule, 
    NgxMatNativeDateModule, 
    NgxMatTimepickerModule 
  ],
  providers: [
    AuthGuardService,
    {provide: HTTP_INTERCEPTORS,useClass: AuthInterceptor,multi: true},
    {provide: RouteReuseStrategy,useClass: RouteReuseService},
    { provide: MAT_DATE_LOCALE, useValue: 'en-nz' },
    {
      provide: NgxMatDateAdapter,
      useClass: CustomNgxDatetimeAdapter,
      deps: [MAT_DATE_LOCALE, NGX_MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    { provide: LAZYLOAD_IMAGE_HOOKS, useClass: LazyLoadImageHooks },
    { provide: NGX_MAT_DATE_FORMATS, useValue: CUSTOM_DATE_FORMATS }
  ],
  exports: [
    CommonModule,
    LazyLoadImageModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    FontAwesomeModule,
    NgFloatingActionMenuModule,
    //ngx-Bootstrap
    BsDropdownModule,
    CollapseModule,
    ModalModule,
    //Material 
    MatCheckboxModule,
    MatCheckboxModule,
    MatButtonModule,
    MatInputModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatStepperModule,
    MatTabsModule,
    MatExpansionModule,
    MatButtonToggleModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatDialogModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    //Components
    DiMatFormEmailComponent,
    DiMatButtonComponent,
    DiMatDatePickerComponent,
    DiMatDateTimePickerComponent,
    DiMatConfirmDialogComponent,
    DiMatIframeDialogComponent,
    DiMatImageDialogComponent,
    DiMatChipComponent,
    FileViewerModalComponent,
    DiBooleanToggleComponent,
    DiMatDateTimePickerComponent,
    //pipes
    DateStringPipe,
    SafePipe,
    SrcStringPipe,
  ]
})
export class SharedModule { }