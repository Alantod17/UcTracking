import { KeyValue } from '@angular/common';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { enumFileType } from 'src/app/shared/elements/di-mat-image-dialog/di-mat-image-dialog.component';
import { CommonService } from 'src/app/shared/services/common.service';
import { SearchFileEntryGroupResultFile } from 'src/app/shared/services/dto';
import { SettingService } from 'src/app/shared/services/setting.service';
declare var $: any;
@Component({
  selector: 'app-gallary-detail',
  templateUrl: './gallary-detail.component.html',
  styleUrls: ['./gallary-detail.component.css']
})
export class GallaryDetailComponent implements OnInit, AfterViewInit {

  fileEntry!: SearchFileEntryGroupResultFile;
  fileGroup!: KeyValue<string, SearchFileEntryGroupResultFile[]>
  fileGroupList: { [key: string]: SearchFileEntryGroupResultFile[] } = {};
  apiEndPoint = SettingService.settings.apiEndPoint;
  fileList: any[] = [];
  fileTypes = enumFileType;

  constructor(private router: Router, private commonService: CommonService) {
    let stateData = this.router.getCurrentNavigation()?.extras.state;
    this.fileEntry = stateData?.fileEntry;
    this.fileGroup = stateData?.fileGroup;
    this.fileGroup.value.forEach(val => {
      let element = { ...val } as any;
      element.fileSafeUrl = `${SettingService.settings.apiEndPoint}file/${val.id}/token?token=${this.commonService.getCurrentUser()?.accessToken}`;
      element.fileType = enumFileType.other;
      if (this.commonService.isImage(val.extension)) {
        element.fileType = enumFileType.image;
      }
      if (this.commonService.isVideo(val.extension)) {
        element.fileType = enumFileType.video;
      }
      this.fileList.push(element);
    })
    this.fileGroupList = stateData?.fileGroupList;
  }

  ngOnInit(): void {
    console.debug(this.fileEntry);
    console.debug(this.fileGroup);
    console.debug(this.fileGroupList);
    console.debug(history.state.data);
  }
  ngAfterViewInit() {
    // $(".fancybox").fancybox({
    //   openEffect: 'none',
    //   closeEffect: 'none',
    //   nextEffect: 'none',
    //   prevEffect: 'none',
    //   padding: 0,
    //   margin: [20, 0, 20, 0]
    // });
  }

}
