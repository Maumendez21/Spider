import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthGuard } from '../../Guards/auth.guard';
import { HomeComponent } from './home.component';
import { PermitsGuard } from '../../Guards/permits.guard';




const routes: Routes = [
  {
    path: 'home',
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    component: HomeComponent,
    loadChildren: () => import('./child-routes.module').then(m => m.ChildRoutesModule)
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule {}
