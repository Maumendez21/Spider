import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeComponent } from './home.component';
import { MapComponent } from './map/map.component';
import { AuthGuard } from '../../Guards/auth.guard';

const routes: Routes = [
    { 
        path: 'home', 
        component: HomeComponent,
        canActivate: [AuthGuard],
        children: [
            {path: '', component: MapComponent}
        ]
    },

    //{ path: 'path/:routeParam', component: MyComponent },
    //{ path: 'staticPath', component: ... },
    //{ path: '**', component: ... },
    //{ path: 'oldPath', redirectTo: '/staticPath' },
    //{ path: ..., component: ..., data: { message: 'Custom' }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HomeRoutingModule {}
