import { GallaryDetailComponent } from './gallary-detail/gallary-detail.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GallaryComponent } from './gallary/gallary.component';


const routes: Routes = [
  { path: '', component: GallaryComponent },
  { path: 'detail', component: GallaryDetailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GallaryRoutingModule { }
