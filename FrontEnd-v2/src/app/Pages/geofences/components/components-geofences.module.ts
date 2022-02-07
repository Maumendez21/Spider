import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfoDeviceMonitoringComponent } from './info-device-monitoring/info-device-monitoring.component';
import { PipesModule } from '../../../Pipes/pipes.module';



@NgModule({
  declarations: [
    InfoDeviceMonitoringComponent
  ],
  exports: [
    InfoDeviceMonitoringComponent

  ],
  imports: [
    CommonModule,
    PipesModule
  ]
})
export class ComponentsGeofencesModule { }
