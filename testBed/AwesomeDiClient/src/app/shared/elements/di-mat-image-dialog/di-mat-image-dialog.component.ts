import { GallaryFile } from './../../services/dto';
import { CommonService } from 'src/app/shared/services/common.service';
import { SettingService } from './../../services/setting.service';
import { SafeUrl } from '@angular/platform-browser';
import { DataService } from './../../services/data.service';
import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { faAngleLeft, faAngleRight, faCircleNotch, faCross, faExpand, faTimes } from '@fortawesome/free-solid-svg-icons';
import { List } from 'linqts';

@Component({
  selector: 'app-di-mat-image-dialog',
  templateUrl: './di-mat-image-dialog.component.html',
  styleUrls: ['./di-mat-image-dialog.component.css']
})
export class DiMatImageDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<DiMatImageDialogComponent>, private domSanitizer: DomSanitizer, @Inject(MAT_DIALOG_DATA) public data: { fileEntry: GallaryFile }, private dataService: DataService, private settingService: SettingService, private commonService: CommonService) {
  }
  @ViewChild('videoPlayer') videoPlayerElement: ElementRef | undefined;
  fileBase64: string | null = null;
  apiEndPoint = SettingService.settings.apiEndPoint;
  fileSafeUrl: string | null = null;
  fileWidth: string | null = "500px";
  fileHeight: string | null = "";
  faSpinnerIcon = faCircleNotch;
  faAngleLeft = faAngleLeft;
  faAngleRight = faAngleRight;
  faExpand = faExpand;
  faTimes = faTimes;
  openImageViewer = false;
  loading = false;
  fileType: enumFileType = enumFileType.other;
  fileTypes = enumFileType;
  // currentId: number | null = null;
  closeDialog(): void {
    this.dialogRef.close();
  }
  ngOnInit(): void {
    // this.LoadFile(this.data.fileEntry);
  }
  private LoadFile(fileEntry: GallaryFile | null) {
    if (fileEntry == null) return;
    this.fileWidth = "50vw";
    this.fileSafeUrl = null;
    this.fileType = enumFileType.other;
    // this.currentId = fileId;
    this.loading = true;
    this.fileSafeUrl = `${SettingService.settings.apiEndPoint}file/${fileEntry.id}/token?token=${this.commonService.getCurrentUser()?.accessToken}`;
    if (this.commonService.isImage(fileEntry.extension)) {
      this.fileType = enumFileType.image;
    }
    if (this.commonService.isVideo(fileEntry.extension)) {
      this.fileType = enumFileType.video;
    }
    if (fileEntry && fileEntry.width && fileEntry.height) {
      let windowHeight = window.screen.height;
      let windowWidth = window.screen.width;
      let height = fileEntry.height;
      let width = fileEntry.width;
      if (width > windowWidth * 0.75) {
        width = windowWidth * 0.75;
        height = fileEntry.height * width / fileEntry.width
        this.fileWidth = width + "px";
        this.fileHeight = "";
      }
      if (windowHeight * 0.75 <= (height)) {
        height = windowHeight * 0.75;
        this.fileHeight = (height - (windowHeight * 0.11)) + "px";
        this.fileWidth = "";
      }
    }
    switch (this.fileType) {
      case enumFileType.other:
        this.loading = false;
        break;
      case enumFileType.video:
        setTimeout(() => {
          this.loading = false;
        }, 500);
        break;
      case enumFileType.image:
        this.loading = false;
        break;
    }
  }
  imageLoaded() {
    this.loading = false;
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
  lastFileClicked() {
    this.dialogRef.close({ showLast: true });
  }
  nextFileClicked() {
    this.dialogRef.close({ showNext: true });
  }

}

export enum enumFileType {
  image,
  video,
  other
}