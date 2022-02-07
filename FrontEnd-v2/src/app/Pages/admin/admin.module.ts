import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersAComponent } from './users-a/users-a.component';
import { DevicesAComponent } from './devices-a/devices-a.component';
import { BulkLoadComponent } from './bulk-load/bulk-load.component';
import { BusinessComponent } from './business/business.component';
import { AlarmsComponent } from './alarms/alarms.component';
import { RoutesComponent } from './routes/routes.component';
import { TreeModule } from '@circlon/angular-tree-component';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from 'src/app/Components/components.module';
import { DataTablesModule } from 'angular-datatables';
import { PipesModule } from 'src/app/Pipes/pipes.module';
import { UiSwitchModule } from 'ngx-toggle-switch';
import { ActionUsersAdComponent } from './users-a/action-users/action-users-ad.component';
import { ActionAssignmentComponent } from './devices-a/action-assignment/action-assignment.component';

@NgModule({
  declarations: [
    UsersAComponent,
    DevicesAComponent,
    BulkLoadComponent,
    BusinessComponent,
    AlarmsComponent,
    RoutesComponent,
    ActionUsersAdComponent,
    ActionAssignmentComponent
  ],
  imports: [
    CommonModule,
    TreeModule,
    FormsModule,
    ComponentsModule,
    DataTablesModule,
    PipesModule,
    UiSwitchModule
  ]
})
export class AdminModule { }
