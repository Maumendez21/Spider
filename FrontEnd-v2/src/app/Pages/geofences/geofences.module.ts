import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonitoringComponent } from './monitoring/monitoring.component';
import { HistoryComponent } from './history/history.component';
import { FormsModule } from '@angular/forms';
import { AgmCoreModule } from '@agm/core';
import { environment } from '../../../environments/environment.prod';
import { ComponentsHomeModule } from '../home/components/components-home.module';
import { ComponentsGeofencesModule } from './components/components-geofences.module';
import { ComponentsModule } from '../../Components/components.module';
import { GeofencesComponent } from './geofences/geofences.component';
import { PipesModule } from '../../Pipes/pipes.module';
import { NewGeofenceComponent } from './geofences/new-geofence/new-geofence.component';
import { RouterModule } from '@angular/router';
import { UpdateGeofenceComponent } from './geofences/update-geofence/update-geofence.component';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';
import { NewAsignationComponent } from './new-asignation/new-asignation.component';
import { DesvinculationComponent } from './desvinculation/desvinculation.component';
import { SharedModule } from '../../Shared/shared.module';



@NgModule({
  declarations: [
    MonitoringComponent,
    HistoryComponent,
    GeofencesComponent,
    NewGeofenceComponent,
    UpdateGeofenceComponent,
    NewAsignationComponent,
    DesvinculationComponent,
  ],
  imports: [
    RouterModule,
    PipesModule,
    CommonModule,
    FormsModule,
    ComponentsGeofencesModule,
    ComponentsModule,
    AgmCoreModule.forRoot({
      apiKey: environment.apiKey,
      libraries: environment.libraries
    }),
    AngularMultiSelectModule,
    SharedModule
  ]
})
export class GeofencesModule { }
