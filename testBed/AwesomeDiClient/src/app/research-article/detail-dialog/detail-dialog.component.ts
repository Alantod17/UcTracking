import { DataService } from 'src/app/shared/services/data.service';
import { GetResearchArticleInfoListResult, GetResearchArticleResult } from './../../shared/services/dto';
import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatAccordion } from '@angular/material/expansion';
import { lastValueFrom } from 'rxjs';
@Component({
  selector: 'detail-dialog',
  templateUrl: './detail-dialog.component.html',
  styleUrls: ['./detail-dialog.component.css']
})
export class DetailDialogComponent implements OnInit {
  loading = true;
  dialogHeight = window.innerHeight - 100;

  @ViewChild(MatAccordion) accordion: MatAccordion = {} as MatAccordion;
  constructor(@Inject(MAT_DIALOG_DATA) public data: DialogData, private dataService: DataService, public dialogRef: MatDialogRef<DetailDialogComponent>) {
    this.article.id = data.articleId;
    this.infoList = data.infoList
  }
  article: GetResearchArticleResult = {} as GetResearchArticleResult;
  infoList: GetResearchArticleInfoListResult = {} as GetResearchArticleInfoListResult;
  ngOnInit() {
    this.dataService.getResearchArticle(this.article.id).subscribe(ra => {
      this.article = ra;
      this.loading = false;
    });
    // this.dataService.getResearchArticleInfoList().subscribe(res => {
    //   this.infoList = res;
    // });
  }
  async save(ra: GetResearchArticleResult) {
    this.loading = true;
    const res = await lastValueFrom(this.dataService.updateResearchArticleResearchDetail(ra.id, { ...ra }));
    this.dialogRef.close(this.article);
  }
  async delete(ra: GetResearchArticleResult) {
    ra.isDeleted = !ra.isDeleted;
    const res = await this.dataService.updateResearchArticleStatus(ra.id, { ...ra }).toPromise();
  }
}

export interface DialogData {
  articleId: number;
  infoList: GetResearchArticleInfoListResult;
}
