import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesComponent } from './pages.component';
import { RouterModule } from '@angular/router';
import { HomeModule } from './home/home.module';
import { SharedModule } from '../Shared/shared.module';
import { LlantasComponent } from './llantas/llantas.component';
import { DetailComponent } from './llantas/detail/detail.component';
import { TrailerSvgComponent } from './llantas/components/trailer-svg/trailer-svg.component';
import { TailDetailComponent } from './llantas/tail-detail/tail-detail.component';

import { NgApexchartsModule } from "ng-apexcharts";



@NgModule({
  declarations: [
    PagesComponent,
    LlantasComponent,
    DetailComponent,
    TrailerSvgComponent,
    TailDetailComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    HomeModule,
    SharedModule,
    NgApexchartsModule
  ]
})
export class PagesModule { }
