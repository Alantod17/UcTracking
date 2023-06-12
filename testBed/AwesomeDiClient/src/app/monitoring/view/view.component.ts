import { firstValueFrom } from 'rxjs';
import { AfterViewInit, Component, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/shared/services/common.service';
import { DataService } from 'src/app/shared/services/data.service';
import { GetTrackingDataParameter, GetTrackingDataResult } from 'src/app/shared/services/dto';
import { MatDialog } from '@angular/material/dialog';
import { ViewDetailDialogComponent } from './view-detail-dialog/view-detail-dialog.component';

@Component({
  selector: 'app-view',
  templateUrl: './view.component.html',
  styleUrls: ['./view.component.css']
})
export class ViewComponent implements OnInit, AfterViewInit {
  stateData: any = null;
  loading: boolean = false;
  dataSource: GetTrackingDataResult[] = [];

  constructor(private router: Router, private commonService: CommonService, private dataService: DataService, private el:ElementRef, public dialog: MatDialog) {
    this.stateData = this.router.getCurrentNavigation()?.extras.state; 

  }
  async ngAfterViewInit(): Promise<void> {
    if(this.stateData){
      try{
        this.loading = true;
        let param = new GetTrackingDataParameter();
        param.trackingId = this.stateData.trackingId;
        param.startDateTime = this.stateData.startDateTime;
        param.endDateTime = this.stateData.endDateTime;
        let trackingDataList = await firstValueFrom(this.dataService.getTrackingData(param));
        this.dataSource = trackingDataList;
      }finally{
        this.loading = false;
      }
    }
  }
  ngOnInit(): void {
  }
  getRequestTitle(data:string){
    let dataDetail = JSON.parse(data);
    return dataDetail.EndPoint;
  }
  getEventTitle(data:string){
    let dataDetail = JSON.parse(data);
    return `Console ${dataDetail.Type}`;
  }
  getUiTitle(data:string){
    let dataDetail = JSON.parse(data);
    return `${dataDetail.EventType}`;
  }
  
  myDrawingFunction(index:number, value:GetTrackingDataResult){
    let dataDetail = JSON.parse(value.dataDetail);
    let image64 = dataDetail.ImageBase64;
    return image64;
  }
  showDetail(value:GetTrackingDataResult){
    this.dialog.open(ViewDetailDialogComponent, {
      data: {...value}
    });
  }
  showImage(value:GetTrackingDataResult){
    let dataDetail = JSON.parse(value.dataDetail);
    let image64 = dataDetail.ImageBase64;
    var image = new Image();
    image.src = image64;

    let w = window.open('','Screenshot','directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width='+dataDetail.PageWidth+',height='+dataDetail.PageHeight);
    if(w){
      w.document.getElementsByTagName("body")[0].innerHTML = "";
      w.document.write(image.outerHTML);
    }

  }
}
