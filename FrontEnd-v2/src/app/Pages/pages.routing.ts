import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeRoutingModule } from './home/home.routing';
import { AnaliticsRoutingModule } from './analitics/analitics.routing';
import { GeofencesRoutingModule } from './geofences/geofences.routing';
import { MobilityRoutingModule } from './mobility/mobility.routing';
import { ConfigurationRoutingModule } from './configuration/configuration.routing';
import { AdminRoutingModule } from './admin/admin.routing';
import { PorfileRoutingModule } from './porfile/profile.routing';
import { MaitananceRoutingModule } from './maintanance/maitanance.routing';


const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {useHash: true}),
    HomeRoutingModule,
    AnaliticsRoutingModule,
    GeofencesRoutingModule,
    MobilityRoutingModule,
    ConfigurationRoutingModule,
    AdminRoutingModule,
    PorfileRoutingModule,
    MaitananceRoutingModule
  ],
  exports: [RouterModule]
})
export class PagesRoutingModule {}
