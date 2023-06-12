import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MonitoringRoutingModule } from './monitoring-routing.module';
import { SearchComponent } from './search/search.component';
import { SharedModule } from '../shared/shared.module';
import { ViewComponent } from './view/view.component';
import { ViewDetailDialogComponent } from './view/view-detail-dialog/view-detail-dialog.component';


@NgModule({
  declarations: [
    SearchComponent,
    ViewComponent,
    ViewDetailDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MonitoringRoutingModule
  ]
})
export class MonitoringModule { }
