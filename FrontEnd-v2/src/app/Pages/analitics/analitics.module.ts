import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DayActivityComponent } from './day-activity/day-activity.component';
import { ComponentsModule } from '../../Components/components.module';
import { AnaliticSharedModule } from './shared/analitic-shared.module';
import { SharedModule } from '../../Shared/shared.module';
import { PerfomanceComponent } from './perfomance/perfomance.component';
import { FormsModule } from '@angular/forms';
import { ComponentsAnaliticsModule } from './components/components-analitics.module';
import { HeathMapComponent } from './heath-map/heath-map.component';
import { PipesModule } from '../../Pipes/pipes.module';
import { AgmCoreModule } from '@agm/core';
import { environment } from '../../../environments/environment.prod';
import { NotificationsAnaliticComponent } from './notifications-analitic/notifications-analitic.component';



@NgModule({
  declarations: [
    DayActivityComponent,
    PerfomanceComponent,
    HeathMapComponent,
    NotificationsAnaliticComponent
  ],
  imports: [
    CommonModule,
    ComponentsModule,
    AnaliticSharedModule,
    SharedModule,
    ComponentsAnaliticsModule,
    FormsModule,
    PipesModule,
    AgmCoreModule.forRoot({
      apiKey: environment.apiKey,
      libraries: environment.libraries
    })
  ]
})
export class AnaliticsModule { }
