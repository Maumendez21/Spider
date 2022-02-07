import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MapComponent } from './map/map.component';
import { RouterModule } from '@angular/router';
import { NotificationComponent } from './notification/notification.component';
import { HomeComponent } from './home.component';
import { SharedModule } from '../../Shared/shared.module';
import { AgmCoreModule, GoogleMapsAPIWrapper, PolygonManager } from '@agm/core';
import { environment } from '../../../environments/environment.prod';
import { ComponentsHomeModule } from './components/components-home.module';
import { SharedHomeModule } from './shared/shared-home.module';


import { PipesModule } from '../../Pipes/pipes.module';
import { UiSwitchModule } from 'ngx-toggle-switch';
import { ComponentsModule } from '../../Components/components.module';
import { ViajeExternoComponent } from './viaje-externo/viaje-externo.component';



@NgModule({
  declarations: [
    MapComponent,
    NotificationComponent,
    HomeComponent,
    ViajeExternoComponent
  ],

  imports: [
    // PipesMapModule,
    CommonModule,
    ComponentsModule,
    RouterModule,
    SharedModule,
    ComponentsHomeModule,
    SharedHomeModule,
    PipesModule,
    UiSwitchModule,
    AgmCoreModule.forRoot({
      apiKey: environment.apiKey,
      libraries: environment.libraries
    })
  ],
})
export class HomeModule { }
