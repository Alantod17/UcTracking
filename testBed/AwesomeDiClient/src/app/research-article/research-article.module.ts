import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResearchArticleRoutingModule } from './research-article-routing.module';
import { SharedModule } from '../shared/shared.module';
import { SearchComponent } from './search/search.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { NgxAnimationsModule } from 'ngx-animations';
import { DetailDialogComponent } from './detail-dialog/detail-dialog.component';


@NgModule({
  declarations: [SearchComponent, DetailDialogComponent],
  imports: [
    CommonModule,
    InfiniteScrollModule,
    ResearchArticleRoutingModule,
    SharedModule, NgxAnimationsModule,
    NgScrollbarModule
  ]
})
export class ResearchArticleModule { }
