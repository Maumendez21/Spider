import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { PagesComponent } from '../pages.component';
import { AuthGuard } from '../../Guards/auth.guard';
import { PermitsGuard } from 'src/app/Guards/permits.guard';


const routes: Routes = [
  {
    path: 'analitics',
    component: PagesComponent ,
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    loadChildren: () => import('./child-routes-analitics.module').then(m => m.ChildRoutesAnaliticsModule)
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AnaliticsRoutingModule {}
