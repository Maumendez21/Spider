import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { PagesComponent } from '../pages.component';
import { AuthGuard } from '../../Guards/auth.guard';


const routes: Routes = [
    { 
        path: 'porfile', 
        component: PagesComponent,
        canActivate: [AuthGuard],
        canLoad: [AuthGuard],
        loadChildren: () => import('./child-routes-porfile.module').then(m => m.ChildRoutesPorfileModule)
    },

    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class PorfileRoutingModule {}
