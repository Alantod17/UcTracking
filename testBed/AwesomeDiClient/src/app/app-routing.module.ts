import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService as AuthGuard } from './shared/services/auth-guard.service';
const routes: Routes = [
  { path: '', loadChildren: () => import('./home/home.module').then(m => m.HomeModule) },
  { path: 'gallary', canActivate: [AuthGuard], loadChildren: () => import('./gallary/gallary.module').then(m => m.GallaryModule) },
  { path: 'monitoring', canActivate: [AuthGuard], loadChildren: () => import('./monitoring/monitoring.module').then(m => m.MonitoringModule) },
  { path: 'research-article', canActivate: [AuthGuard], loadChildren: () => import('./research-article/research-article.module').then(m => m.ResearchArticleModule) },
  { path: 'sharesies', canActivate: [AuthGuard], loadChildren: () => import('./sharesies/sharesies.module').then(m => m.SharesiesModule) },
  // { path: 'research-article', canActivate: [AuthGuard], data: { roles: ["Researcher", "Admin"] }, loadChildren: () => import('./research-article/research-article.module').then(m => m.ResearchArticleModule) },
  { path: 'home', loadChildren: () => import('./home/home.module').then(m => m.HomeModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
