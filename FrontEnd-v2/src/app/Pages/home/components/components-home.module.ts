import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfoMDeviceModalComponent } from './modals/info-mdevice-modal/info-mdevice-modal.component';
import { SharedHomeModule } from '../shared/shared-home.module';
import { PipesModule } from 'src/app/Pipes/pipes.module';
import { VelocityGraphicComponent } from './velocity-graphic/velocity-graphic.component';
import { ChartsModule } from 'ng2-charts';
import { GraphKmRecorridosComponent } from './modals/graph-km-recorridos/graph-km-recorridos.component';
import { GraphTimeActivityComponent } from './modals/graph-time-activity/graph-time-activity.component';
import { GraphFuelConsumeComponent } from './modals/graph-fuel-consume/graph-fuel-consume.component';



@NgModule({
  declarations: [
    InfoMDeviceModalComponent,
    VelocityGraphicComponent,
    GraphKmRecorridosComponent,
    GraphTimeActivityComponent,
    GraphFuelConsumeComponent
  ],
  exports: [
    InfoMDeviceModalComponent,
    VelocityGraphicComponent,
    GraphKmRecorridosComponent,
    GraphTimeActivityComponent,
    GraphFuelConsumeComponent

  ],
  imports: [
    PipesModule,
    CommonModule,
    SharedHomeModule,
    ChartsModule

  ]
})
export class ComponentsHomeModule { }
