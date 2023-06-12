import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GallaryRoutingModule } from './gallary-routing.module';
import { SharedModule } from '../shared/shared.module';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { GallaryComponent } from './gallary/gallary.component';
import { GallaryDetailComponent } from './gallary-detail/gallary-detail.component';


@NgModule({
  declarations: [GallaryComponent, GallaryDetailComponent],
  imports: [
    CommonModule,
    InfiniteScrollModule,
    GallaryRoutingModule,
    SharedModule,
  ]
})
export class GallaryModule { }
