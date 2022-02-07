import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { PagesComponent } from '../pages.component';
import { AuthGuard } from '../../Guards/auth.guard';



const routes: Routes = [
    { 
        path: 'mobility', 
        component: PagesComponent,
        canActivate: [AuthGuard],
        canLoad: [AuthGuard],
        loadChildren: () => import('./child-routes-mobility.module').then(m => m.ChildRoutesMobilityModule)

    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MobilityRoutingModule {}
