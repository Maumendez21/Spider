import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { MonitoringComponent } from './monitoring/monitoring.component';
import { HistoryComponent } from './history/history.component';
import { GeofencesComponent } from './geofences/geofences.component';
import { NewGeofenceComponent } from './geofences/new-geofence/new-geofence.component';
import { UpdateGeofenceComponent } from './geofences/update-geofence/update-geofence.component';
import { NewAsignationComponent } from './new-asignation/new-asignation.component';
import { DesvinculationComponent } from './desvinculation/desvinculation.component';


const childRoutes: Routes = [

  { path: 'list', component: GeofencesComponent,  data: { title: 'Geocercas' }},
  { path: 'new', component: NewGeofenceComponent,  data: { title: 'Crear Geocerca' }},
  { path: 'new-asignation', component: NewAsignationComponent,  data: { title: 'Nueva asignación' }},
  { path: 'desvinculation', component: DesvinculationComponent,  data: { title: 'Desvincular' }},
  { path: 'geofence/:id', component: UpdateGeofenceComponent,  data: { title: 'Actualizar Geocerca' }},
  { path: 'monitoring', component: MonitoringComponent,  data: { title: 'Monitoreo de Geocercas' }},
  { path: 'history', component: HistoryComponent,  data: { title: 'Histórico Geocercas' }},
]



@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class ChildRoutesGeofencesModule { }
