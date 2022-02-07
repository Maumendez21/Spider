import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActividatHoyTabComponent } from './tabs/actividat-hoy-tab/actividat-hoy-tab.component';
import { PipesModule } from '../../../Pipes/pipes.module';
import { DetalleTabComponent } from './tabs/detalle-tab/detalle-tab.component';
import { Detalle2TabComponent } from './tabs/detalle2-tab/detalle2-tab.component';
import { AlarmasTabComponent } from './tabs/alarmas-tab/alarmas-tab.component';
import { AgmCoreModule } from '@agm/core';
import { environment } from '../../../../environments/environment.prod';
import { ReporteTabComponent } from './tabs/reporte-tab/reporte-tab.component';
import { FormsModule } from '@angular/forms';
import { ParoMotorTabComponent } from './tabs/paro-motor-tab/paro-motor-tab.component';
import { CleanFiltersComponent } from './clean-filters/clean-filters.component';
import { DetailsTravelComponent } from './details-travel/details-travel.component';



@NgModule({
  declarations: [
    ActividatHoyTabComponent,
    DetalleTabComponent,
    Detalle2TabComponent,
    AlarmasTabComponent,
    ReporteTabComponent,
    ParoMotorTabComponent,
    CleanFiltersComponent,
    DetailsTravelComponent
  ],
  exports: [
    ActividatHoyTabComponent,
    DetalleTabComponent,
    Detalle2TabComponent,
    AlarmasTabComponent,
    ReporteTabComponent,
    ParoMotorTabComponent,
    CleanFiltersComponent,
    DetailsTravelComponent

  ],
  imports: [
    CommonModule,
    PipesModule,
    FormsModule,
    AgmCoreModule.forRoot({
      apiKey: environment.apiKey,
      libraries: environment.libraries
    })
  ]
})
export class SharedHomeModule { }
