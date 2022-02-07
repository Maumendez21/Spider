import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RutesComponent } from './rutes/rutes.component';
import { RouterModule, Routes } from '@angular/router';
import { PoinstInterestComponent } from './poinst-interest/poinst-interest.component';
import { ItinerariesResponsablesComponent } from './itineraries-responsables/itineraries-responsables.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { NewRouteComponent } from './rutes/new-route/new-route.component';
import { UpdateRouteComponent } from './rutes/update-route/update-route.component';
import { ConsultPointsInterestComponent } from './poinst-interest/consult-points-interest/consult-points-interest.component';
import { ActionPointInterestComponent } from './poinst-interest/action-point-interest/action-point-interest.component';
import { PointVinculationComponent } from './point-vinculation/point-vinculation.component';
import { PointDesvinculationComponent } from './point-desvinculation/point-desvinculation.component';

const childRoutes: Routes = [
  { path: 'rutes', component: RutesComponent,  data: { title: 'Rutas' }},
  { path: 'new-route', component: NewRouteComponent,  data: { title: 'Crear Ruta' }},
  { path: 'route/:id', component: UpdateRouteComponent,  data: { title: 'Ver Ruta' }},
  { path: 'points-interest', component: PoinstInterestComponent,  data: { title: 'Puntos de Interés' }},
  { path: 'point-interest/:id', component: ActionPointInterestComponent,  data: { title: 'Punto de interés' }},
  { path: 'point-vinculation', component: PointVinculationComponent,  data: { title: 'Asignación Punto de Interés' }},
  { path: 'point-desvinculation', component: PointDesvinculationComponent,  data: { title: 'Eliminar Asignación Punto de Interés' }},
  { path: 'points-interest/consult', component: ConsultPointsInterestComponent,  data: { title: 'Consultar Punto de Interés' }},
  { path: 'itineraries-responsable', component: ItinerariesResponsablesComponent,  data: { title: 'Itinerarios Responsables' }},
  { path: 'schedule', component: ScheduleComponent,  data: { title: 'Agenda' }},
]

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class ChildRoutesMobilityModule { }
