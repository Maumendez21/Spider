import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { PagesComponent } from '../pages.component';
import { AuthGuard } from '../../Guards/auth.guard';


const routes: Routes = [
    { 
        path: 'administration', 
        component: PagesComponent,
        canActivate: [AuthGuard],
        canLoad: [AuthGuard],
        loadChildren: () => import('./child-routes-admin.module').then(m => m.ChildRoutesAdminModule)
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminRoutingModule {}
