import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GetTrackingDataResult } from 'src/app/shared/services/dto';

@Component({
  selector: 'app-view-detail-dialog',
  templateUrl: './view-detail-dialog.component.html',
  styleUrls: ['./view-detail-dialog.component.css']
})
export class ViewDetailDialogComponent implements OnInit {
  trackingData: GetTrackingDataResult = {
    type: '',
    trackingId: '',
    dateTime: '',
    dataDetail: ''
  };
  
  dialogHeight = window.innerHeight - 100;
  constructor(@Inject(MAT_DIALOG_DATA) public data: GetTrackingDataResult, public dialogRef: MatDialogRef<ViewDetailDialogComponent>) {
    this.trackingData = data;
  }

  ngOnInit(): void {
  }
  getJsonText(input:string){
    let value = JSON.parse(input);
    return JSON.stringify(value, null, 2);
  }
}
