import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharesiesRoutingModule } from './sharesies-routing.module';
import { SharedModule } from '../shared/shared.module';
import { NgxAnimationsModule } from 'ngx-animations';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { SearchComponent } from './search/search.component';


@NgModule({
  declarations: [
    SearchComponent
  ],
  imports: [
    CommonModule,
    SharesiesRoutingModule,
    SharedModule, 
    NgxAnimationsModule,
    NgScrollbarModule
  ]
})
export class SharesiesModule { }
