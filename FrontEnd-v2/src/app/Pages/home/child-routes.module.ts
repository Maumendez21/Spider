import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MapComponent } from './map/map.component';
import { NotificationComponent } from './notification/notification.component';
import { ViajeExternoComponent } from './viaje-externo/viaje-externo.component';

const childRoutes: Routes = [
  { path: '', component: MapComponent},
  { path: 'notification', component: NotificationComponent,  data: { title: 'Notificaciones' } },
  { path: 'trip/:device/:startDate/:endDate', component: ViajeExternoComponent,  data: { title: 'Detalle de Viaje' }},
]


@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class ChildRoutesModule { }
