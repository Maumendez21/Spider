import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { RightMenuComponent } from './right-menu/right-menu.component';
import { TravelsComponent } from './right-menu/travels/travels.component';
import { ConfigMapComponent } from './right-menu/config-map/config-map.component';
import { FormsModule } from '@angular/forms';
import { FiltroFechasTravelsComponent } from './right-menu/filtro-fechas-travels/filtro-fechas-travels.component';
import { PipesModule } from '../Pipes/pipes.module';
import { FilterStatusComponent } from './right-menu/filter-status/filter-status.component';
import { OptionsMapComponent } from './right-menu/options-map/options-map.component';
import { UiSwitchModule } from 'ngx-toggle-switch';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { LoadingComponent } from './loading/loading.component';
import { ComponentsModule } from '../Components/components.module';


@NgModule({
  declarations: [
    SidebarComponent,
    HeaderComponent,
    RightMenuComponent,
    TravelsComponent,
    ConfigMapComponent,
    FiltroFechasTravelsComponent,
    FilterStatusComponent,
    OptionsMapComponent,
    BreadcrumbComponent,
    LoadingComponent,
  ],
  exports: [
    SidebarComponent,
    HeaderComponent,
    RightMenuComponent,
    BreadcrumbComponent,
    LoadingComponent,

  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    PipesModule,
    UiSwitchModule,
    ComponentsModule
  ]
})
export class SharedModule { }
