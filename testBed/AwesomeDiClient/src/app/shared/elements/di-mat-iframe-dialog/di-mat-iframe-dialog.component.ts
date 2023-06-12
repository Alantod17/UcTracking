import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { DiMatConfirmDialogComponent } from '../di-mat-confirm-dialog/di-mat-confirm-dialog/di-mat-confirm-dialog.component';

@Component({
  selector: 'app-di-mat-iframe-dialog',
  templateUrl: './di-mat-iframe-dialog.component.html',
  styleUrls: ['./di-mat-iframe-dialog.component.css']
})
export class DiMatIframeDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<DiMatIframeDialogComponent>, 
    private http:HttpClient,
    private sanitizer:DomSanitizer,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
