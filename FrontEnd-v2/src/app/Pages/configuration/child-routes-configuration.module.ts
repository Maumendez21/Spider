import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { GroupsComponent } from './groups/groups.component';
import { DevicesCComponent } from './devices-c/devices-c.component';
import { ResponsablesComponent } from './responsables/responsables.component';
import { UsersCComponent } from './users-c/users-c.component';
import { InfoVehicleComponent } from './info-vehicle/info-vehicle.component';
import { InfoDeviceComponent } from './info-device/info-device.component';
import { GeneralParametersComponent } from './general-parameters/general-parameters.component';
import { ScheduleCComponent } from './schedule-c/schedule-c.component';
import { EngineStopComponent } from './engine-stop/engine-stop.component';


const childRoutes: Routes = [

  { path: 'groups', component: GroupsComponent,  data: { title: 'Grupos' }},
  { path: 'schedule', component: ScheduleCComponent,  data: { title: 'Agenda' }},
  { path: 'devices', component: DevicesCComponent,  data: { title: 'Dispositivos' }},
  { path: 'responsables', component: ResponsablesComponent,  data: { title: 'Responsables' }},
  { path: 'users', component: UsersCComponent,  data: { title: 'Usuarios' }},
  { path: 'infoVehicles', component: InfoVehicleComponent,  data: { title: 'Información de vehiculos' }},
  { path: 'infoDevices', component: InfoDeviceComponent,  data: { title: 'Información de Dispositivos' }},
  { path: 'parameters', component: GeneralParametersComponent,  data: { title: 'Parametros Generales' }},
  { path: 'engine-stop', component: EngineStopComponent,  data: { title: 'Paro de Motor' }},
  
]




@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class ChildRoutesConfigurationModule { }
