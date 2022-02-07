import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DayActivityComponent } from './day-activity/day-activity.component';
import { HeathMapComponent } from './heath-map/heath-map.component';
import { PerfomanceComponent } from './perfomance/perfomance.component';
import { NotificationsAnaliticComponent } from './notifications-analitic/notifications-analitic.component';

const childRoutes: Routes = [
  { path: 'day-activity', component: DayActivityComponent,  data: { title: 'Actividad de Hoy' }},
  { path: 'perfomance', component: PerfomanceComponent,  data: { title: 'Rendimiento' }},
  { path: 'heath-map', component: HeathMapComponent,  data: { title: 'Mapa de calor' }},
  { path: 'notifications', component: NotificationsAnaliticComponent,  data: { title: 'Analítica Notificaciones' }},
]

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class ChildRoutesAnaliticsModule { }
