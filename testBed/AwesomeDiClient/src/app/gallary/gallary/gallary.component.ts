
import { SettingService } from './../../shared/services/setting.service';
import { FileViewerModalComponent } from './../../shared/elements/file-viewer-modal/file-viewer-modal.component';
import { SearchFileEntryGroupResultFile, SearchFileEntryGroupParameter } from './../../shared/services/dto';
import { DataService } from './../../shared/services/data.service';
import { AfterViewInit, Component, OnInit, Renderer2 } from '@angular/core';
import { faCircleNotch } from '@fortawesome/free-solid-svg-icons';
import { CommonService } from '../../shared/services/common.service';
import { KeyValue } from '@angular/common';
import { FloatingActionButton } from 'ng-floating-action-menu';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';
import { enumFileType } from 'src/app/shared/elements/di-mat-image-dialog/di-mat-image-dialog.component';
declare var $: any;
@Component({
  selector: 'app-gallary',
  templateUrl: './gallary.component.html',
  styleUrls: ['./gallary.component.css']
})
export class GallaryComponent implements OnInit, AfterViewInit {

  constructor(private dataService: DataService, private commonService: CommonService, private modalService: BsModalService, private router: Router) {
    if (window.innerWidth > 768) {
      this.photoSize = (window.innerWidth * 0.1) + "";
    } else {
      this.photoSize = (window.innerWidth * 0.2) + "";
    }
  }
  fileTypes = enumFileType;
  modalRef!: BsModalRef;
  groupedArray: any[] = [];
  showType: string = "Day"
  faSpinnerIcon = faCircleNotch;
  photoSize = (window.innerWidth * 0.1) + "";
  loading = false;
  onScrollLoading = false;
  apiEndPoint = SettingService.settings.apiEndPoint;
  startUtcDate: string = "";
  endUtcDate: string = "";
  groupBy = "Date";
  accessToken = this.commonService.getCurrentUser()?.accessToken;
  fileGroupList: { [key: string]: SearchFileEntryGroupResultFile[] } = {};
  async loadFile(startUtcDateStr: string, endUtcDateStr: string) {
    let param: SearchFileEntryGroupParameter = { startUtcDate: startUtcDateStr, endUtcDate: endUtcDateStr, groupBy: this.groupBy };
    let res = await this.dataService.searchFileEntryGroup(param).toPromise();
    if (res && res.length > 0) {
      res.forEach(fileGroup => {
        if (!this.fileGroupList[fileGroup.groupKey]) {
          this.fileGroupList[fileGroup.groupKey] = [];
        }
        fileGroup.fileList.forEach(fileEntry => {
          this.fileGroupList[fileGroup.groupKey].push(fileEntry);
        });
      });
    }
  }
  async ngOnInit(): Promise<void> {
    this.loading = true;
    let now = this.commonService.getNowDateTimeString();
    this.endUtcDate = this.commonService.getUtcDateString(now);
    this.startUtcDate = this.commonService.getUtcDateString(this.commonService.addDays(now, -60));
    await this.loadFile(this.startUtcDate, this.endUtcDate);
    this.loading = false;


  }
  async onScroll() {
    if (this.onScrollLoading == false) {
      this.onScrollLoading = true;
      let endUtcDate = this.startUtcDate;
      let startUtcDate = this.commonService.getUtcDateString(this.commonService.addDays(this.startUtcDate, -60));
      this.startUtcDate = startUtcDate;
      await this.loadFile(startUtcDate, endUtcDate);
      this.onScrollLoading = false;
    }
  }
  async scrolledUp() {
    if (this.onScrollLoading == false) {
      this.onScrollLoading = true;
      let startUtcDate = this.endUtcDate;
      let endUtcDate = this.commonService.getUtcDateString(this.commonService.addDays(startUtcDate, 30));
      this.endUtcDate = endUtcDate;
      await this.loadFile(startUtcDate, endUtcDate);
      this.onScrollLoading = false;
    }
  }
  openModal(fileEntry: SearchFileEntryGroupResultFile, fileGroup: KeyValue<string, SearchFileEntryGroupResultFile[]>) {
    const initialState = {
      fileGroup: fileGroup,
      fileGroupList: this.fileGroupList,
      fileEntry: fileEntry
    };
    this.router.navigate(['gallary/detail'], { state: initialState });
    // const config = {
    //   "class": 'modal-dialog-centered',
    //   "initialState": initialState
    // }
    // this.modalRef = this.modalService.show(FileViewerModalComponent, config);
    // this.modalRef.content.closeBtnName = 'Close';
  }
  buttons: Array<FloatingActionButton> = [
    {
      iconClass: 'fas fa-angle-double-up',
      label: 'Back To Top',
      onClick: () => {
        window.scrollTo({
          top: 0,
          left: 0,
          behavior: 'smooth'
        });
      }
    },
    {
      iconClass: 'fas fa-th-large',
      label: 'Date view',
      onClick: async () => {
        let multipler = window.innerWidth > 768 ? 0.1 : 0.2;
        this.photoSize = (window.innerWidth * multipler) + "";
        this.groupBy = "Date";
        await this.reloadFileList();
      }
    },
    {
      iconClass: 'fas fa-grip-horizontal',
      label: 'Month view',
      onClick: async () => {
        let multipler = window.innerWidth > 768 ? 0.05 : 0.15;
        this.photoSize = (window.innerWidth * multipler) + "";
        this.groupBy = "Month";
        await this.reloadFileList();
      }
    },
    {
      iconClass: 'fas fa-th',
      label: 'Year view',
      onClick: async () => {
        let multipler = window.innerWidth > 768 ? 0.025 : 0.1;
        this.photoSize = (window.innerWidth * multipler) + "";
        this.groupBy = "Year";
        await this.reloadFileList();
      }
    },
  ];
  async reloadFileList() {
    this.loading = true;
    this.fileGroupList = {};
    let now = this.commonService.getNowDateTimeString();
    this.endUtcDate = this.commonService.getUtcDateString(now);
    this.startUtcDate = this.commonService.getUtcDateString(this.commonService.addDays(now, -60));
    if (this.groupBy == "Year") {
      this.startUtcDate = this.commonService.getUtcDateString(this.commonService.addDays(now, -365 * 40));
    }
    await this.loadFile(this.startUtcDate, this.endUtcDate);
    this.loading = false;
  }

  // Order by descending property key
  keyDescOrder = (a: KeyValue<string, SearchFileEntryGroupResultFile[]>, b: KeyValue<string, SearchFileEntryGroupResultFile[]>): number => {
    return a.key > b.key ? -1 : (b.key > a.key ? 1 : 0);
  }
  getFileSafeUrl(file: SearchFileEntryGroupResultFile) {
    console.debug("FileURL for:" + file.id);
    return `${SettingService.settings.apiEndPoint}file/${file.id}/token?token=${this.commonService.getCurrentUser()?.accessToken}`;
  }
  getFileType(file: SearchFileEntryGroupResultFile) {
    let fileType = enumFileType.other;
    if (this.commonService.isImage(file.extension)) {
      fileType = enumFileType.image;
    }
    if (this.commonService.isVideo(file.extension)) {
      fileType = enumFileType.video;
    }
    return fileType;
  }

  ngAfterViewInit() {
    $.fancybox.defaults.video = {
      tpl:
        '<video class="fancybox-video" controls controlsList="nodownload" poster="{{poster}}" preload="auto">' +
        '<source src="{{src}}" type="{{format}}" />' +
        'Sorry, your browser doesn\'t support embedded videos, <a href="{{src}}">download</a> and watch with your favorite video player!' +
        "</video>",
      format: "", // custom video format
      autoStart: false
    }
  }
  getFileWithAndHeight(fileEntry: SearchFileEntryGroupResultFile): any {
    let res = { width: 300, height: 300 };
    if (fileEntry && fileEntry.width && fileEntry.height) {
      let windowHeight = window.screen.height;
      let windowWidth = window.screen.width;
      let height = fileEntry.height;
      let width = fileEntry.width;
      let imgPercent = 1;
      if (width > windowWidth * imgPercent) {
        width = windowWidth * imgPercent;
        height = fileEntry.height * width / fileEntry.width;
        res.width = width;
        res.height = height
      }
      if (windowHeight * imgPercent <= (height)) {
        height = windowHeight * imgPercent;
        height = (height - (windowHeight * 0.11));
        res.height = height;
        res.width = fileEntry.width * height / fileEntry.height;;
      }
      return res;
    }
  }
}

