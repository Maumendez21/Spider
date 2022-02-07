import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupsComponent } from './groups/groups.component';
import { TreeModule } from '@circlon/angular-tree-component';
import { FormsModule } from '@angular/forms';
import { DevicesCComponent } from './devices-c/devices-c.component';
import { DataTablesModule } from 'angular-datatables';
import { PipesModule } from 'src/app/Pipes/pipes.module';
import { ResponsablesComponent } from './responsables/responsables.component';
import { ComponentsModule } from '../../Components/components.module';
import { EditResponsableComponent } from './responsables/edit-responsable/edit-responsable.component';
import { UsersCComponent } from './users-c/users-c.component';
import { ActionUsersComponent } from './users-c/action-users/action-users.component';
import { PermissionsUsersComponent } from './users-c/permissions-users/permissions-users.component';
import { InfoVehicleComponent } from './info-vehicle/info-vehicle.component';
import { UpdateInfoVehicleComponent } from './info-vehicle/update-info-vehicle/update-info-vehicle.component';
import { AddVersionComponent } from './info-vehicle/add-version/add-version.component';
import { InfoDeviceComponent } from './info-device/info-device.component';
import { UpdateInfoDeviceComponent } from './info-device/update-info-device/update-info-device.component';
import { GeneralParametersComponent } from './general-parameters/general-parameters.component';
import { UpdateParametersComponent } from './general-parameters/update-parameters/update-parameters.component';
import { ScheduleCComponent } from './schedule-c/schedule-c.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import { ComponentsConfigurationModule } from './components/components-configuration.module';
import { EngineStopComponent } from './engine-stop/engine-stop.component';


@NgModule({
  declarations: [
    GroupsComponent,
    DevicesCComponent,
    ResponsablesComponent,
    EditResponsableComponent,
    UsersCComponent,
    ActionUsersComponent,
    PermissionsUsersComponent,
    InfoVehicleComponent,
    UpdateInfoVehicleComponent,
    AddVersionComponent,
    InfoDeviceComponent,
    UpdateInfoDeviceComponent,
    GeneralParametersComponent,
    UpdateParametersComponent,
    ScheduleCComponent,
    EngineStopComponent
  ],
  imports: [
    FullCalendarModule,
    CommonModule,
    TreeModule,
    FormsModule,
    ComponentsModule,
    DataTablesModule,
    PipesModule,
    ComponentsConfigurationModule
  ]
})
export class ConfigurationModule { }
