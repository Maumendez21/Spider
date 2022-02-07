import { NgModule }             from '@angular/core';
import { UsersAComponent }      from './users-a/users-a.component';
import { Routes, RouterModule } from '@angular/router';
import { DevicesAComponent }    from './devices-a/devices-a.component';
import { BulkLoadComponent }    from './bulk-load/bulk-load.component';
import { BusinessComponent }    from './business/business.component';
import { AlarmsComponent }      from './alarms/alarms.component';
import { RoutesComponent }      from './routes/routes.component';

const childRoutes: Routes = [
  { path: 'users', component: UsersAComponent,  data: { title: 'Usuarios' }},
  { path: 'devices', component: DevicesAComponent,  data: { title: 'Devices' }},
  { path: 'bulk-load', component: BulkLoadComponent,  data: { title: 'Carga Masiva' }},
  { path: 'business', component: BusinessComponent,  data: { title: 'Empresas' }},
  { path: 'alarms', component: AlarmsComponent,  data: { title: 'Alarmas' }},
  { path: 'routes', component: RoutesComponent,  data: { title: 'Rutas' }},
]

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class ChildRoutesAdminModule { }
