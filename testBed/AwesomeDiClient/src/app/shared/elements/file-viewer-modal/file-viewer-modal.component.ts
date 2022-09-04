import { DataService } from './../../services/data.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { faAngleLeft, faAngleRight, faCircleNotch, faExpand, faTimes } from '@fortawesome/free-solid-svg-icons';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CommonService } from '../../services/common.service';
import { GallaryFile, SearchFileEntryGroupResultFile } from '../../services/dto';
import { SettingService } from '../../services/setting.service';
import { enumFileType } from '../di-mat-image-dialog/di-mat-image-dialog.component';
import { KeyValue } from '@angular/common';

@Component({
  selector: 'app-file-viewer-modal',
  templateUrl: './file-viewer-modal.component.html',
  styleUrls: ['./file-viewer-modal.component.css']
})
export class FileViewerModalComponent implements OnInit {

  faSpinnerIcon = faCircleNotch;
  faAngleLeft = faAngleLeft;
  faAngleRight = faAngleRight;
  faExpand = faExpand;
  faTimes = faTimes;
  title!: string;
  closeBtnName!: string;
  list: any[] = [];
  fileWidth: string | null = "500px";
  fileHeight: string | null = "";
  fileSafeUrl: string | null = null;
  loading = false;
  openImageViewer = false;
  fileType: enumFileType = enumFileType.other;
  fileTypes = enumFileType;
  @ViewChild('videoPlayer') videoPlayerElement: ElementRef | undefined;
  fileMargin: number = 0;
  modalHeight: number = window.innerHeight * 0.8;
  fileEntry!: SearchFileEntryGroupResultFile;
  fileGroup!: KeyValue<string, SearchFileEntryGroupResultFile[]>
  fileGroupList: { [key: string]: SearchFileEntryGroupResultFile[] } = {};

  constructor(public bsModalRef: BsModalRef, private commonService: CommonService, private dataService: DataService) { }

  ngOnInit() {
    this.LoadFile();
  }

  imageLoaded() {
    this.loading = false;
  }
  // private LoadFile(fileEntry: GallaryFile | null) {
  private async LoadFile() {
    // if (fileEntry == null) return;
    if (!this.fileEntry?.id) return;
    this.fileWidth = "50vw";
    this.fileSafeUrl = null;
    this.fileType = enumFileType.other;
    this.loading = true;
    let fileEntry = await this.dataService.getFileEntry({ id: this.fileEntry.id }).toPromise();
    if (!fileEntry) return;
    this.fileSafeUrl = `${SettingService.settings.apiEndPoint}file/${fileEntry.id}/token?token=${this.commonService.getCurrentUser()?.accessToken}`;
    if (this.commonService.isImage(fileEntry.extension)) {
      this.fileType = enumFileType.image;
    }
    if (this.commonService.isVideo(fileEntry.extension)) {
      this.fileType = enumFileType.video;
    }
    if (fileEntry && fileEntry.fileWidth && fileEntry.fileHeight) {
      let windowHeight = window.screen.height;
      let windowWidth = window.screen.width;
      let height = fileEntry.fileHeight;
      let width = fileEntry.fileWidth;
      let imgPercent = 1;
      if (width > windowWidth * imgPercent) {
        width = windowWidth * imgPercent;
        height = fileEntry.fileHeight * width / fileEntry.fileWidth;
        this.fileWidth = width + "px";
        this.fileHeight = "";
      }
      if (windowHeight * imgPercent <= (height)) {
        height = windowHeight * imgPercent;
        height = (height - (windowHeight * 0.11));
        this.fileHeight = height + "px";
        this.fileWidth = "";
      }
    }
    if (this.fileType != enumFileType.image) {
      this.loading = false;
    }
  }

  viewInNewTab() {
    if (this.fileType == enumFileType.image) {
      this.openImageViewer = true;
    } else if (this.fileType == enumFileType.video) {
      if (this.videoPlayerElement?.nativeElement.requestFullscreen) {
        this.videoPlayerElement?.nativeElement.requestFullscreen();
      } else if (this.videoPlayerElement?.nativeElement.mozRequestFullScreen) {
        this.videoPlayerElement?.nativeElement.mozRequestFullScreen();
      } else if (this.videoPlayerElement?.nativeElement.webkitRequestFullscreen) {
        this.videoPlayerElement?.nativeElement.webkitRequestFullscreen();
      } else if (this.videoPlayerElement?.nativeElement.msRequestFullscreen) {
        this.videoPlayerElement?.nativeElement.msRequestFullscreen();
      }
    }
  }
  async lastFileClicked() {
    let fileList = this.fileGroup.value;
    let index = fileList.indexOf(this.fileEntry);
    if (index > 0) {
      let newIndex = index - 1;
      this.fileEntry = fileList[newIndex];
      await this.LoadFile();
    } else {
      let properties = Object.getOwnPropertyNames(this.fileGroupList);
      let groupIndex = properties.indexOf(this.fileGroup.key);
      if (groupIndex > 0) {
        let nextGroupKey: string = properties[groupIndex - 1];
        let nextGroupValue = this.fileGroupList[nextGroupKey];
        this.fileGroup = { key: nextGroupKey, value: nextGroupValue };
        this.fileEntry = nextGroupValue[nextGroupValue.length - 1];
        await this.LoadFile();
      } else {
        this.bsModalRef.hide()
      }
    }
  }
  async nextFileClicked() {
    let fileList = this.fileGroup.value;
    let index = fileList.indexOf(this.fileEntry);
    if (index < fileList.length - 1) {
      let newIndex = index + 1;
      this.fileEntry = fileList[newIndex];
      await this.LoadFile();
    } else {
      let properties = Object.getOwnPropertyNames(this.fileGroupList);
      let groupIndex = properties.indexOf(this.fileGroup.key);
      if (groupIndex < properties.length - 1) {
        let nextGroupKey: string = properties[groupIndex + 1];
        let nextGroupValue = this.fileGroupList[nextGroupKey];
        this.fileGroup = { key: nextGroupKey, value: nextGroupValue };
        this.fileEntry = nextGroupValue[0];
        await this.LoadFile();
      } else {
        this.bsModalRef.hide();
        //last image (close modal)
      }
    }
  }

}
