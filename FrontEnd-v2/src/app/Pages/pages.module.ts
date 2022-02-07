import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesComponent } from './pages.component';
import { SharedModule } from '../Shared/shared.module';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '../app-routing.module';
import { HomeModule } from './home/home.module';
import { AnaliticsModule } from './analitics/analitics.module';
import { GeofencesModule } from './geofences/geofences.module';
import { MobilityModule } from './mobility/mobility.module';
import { ConfigurationModule } from './configuration/configuration.module';
import { AdminModule } from './admin/admin.module';
import { PorfileModule } from './porfile/porfile.module';
import { MaitananceModule } from './maintanance/maitanance.module';


@NgModule({
  declarations: [
    PagesComponent,
  ],
  imports: [
    AppRoutingModule,
    RouterModule,
    CommonModule,
    SharedModule,
    HomeModule,
    AnaliticsModule,
    GeofencesModule,
    MobilityModule,
    ConfigurationModule,
    AdminModule,
    PorfileModule,
    MaitananceModule
  ]
})
export class PagesModule { }
