import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'di-mat-confirm-dialog',
  templateUrl: './di-mat-confirm-dialog.component.html',
  styleUrls: ['./di-mat-confirm-dialog.component.css']
})
export class DiMatConfirmDialogComponent {

  constructor(public dialogRef: MatDialogRef<DiMatConfirmDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  closeDialog(): void {
    this.dialogRef.close();
  }

}
