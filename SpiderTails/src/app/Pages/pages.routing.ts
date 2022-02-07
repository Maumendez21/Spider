import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeRoutingModule } from './home/home.routing';
import { LlantasComponent } from './llantas/llantas.component';
import { AuthGuard } from '../Guards/auth.guard';
import { DetailComponent } from './llantas/detail/detail.component';
import { PagesComponent } from './pages.component';
import { TailDetailComponent } from './llantas/tail-detail/tail-detail.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full'
    },
    {
        path: 'tails',
        component: PagesComponent,
        canActivate: [AuthGuard],
        children: [
            {path: '', component: LlantasComponent, data: { title: 'Camiones' } },
            {path: ':id', component: DetailComponent,data: { title: 'Detalle de camión' }},
            {path: ':id/tail/:idTail', component: TailDetailComponent,data: { title: 'Detalle de llanta' }}
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes, {useHash: true}),
        HomeRoutingModule
    ],
    exports: [RouterModule]
})
export class PagesRoutingModule {}
