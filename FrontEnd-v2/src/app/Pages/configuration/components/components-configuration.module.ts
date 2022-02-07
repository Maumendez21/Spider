import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScheduleOptionsComponent } from './modals/schedule-options/schedule-options.component';
import { NewEventResponsableComponent } from './modals/new-event-responsable/new-event-responsable.component';
import { NewEventRouteComponent } from './modals/new-event-route/new-event-route.component';
import { UpdateEventRouteComponent } from './modals/update-event-route/update-event-route.component';
import { UpdateEventResponsableComponent } from './modals/update-event-responsable/update-event-responsable.component';
import { FormsModule } from '@angular/forms';
import { PipesModule } from 'src/app/Pipes/pipes.module';



@NgModule({
  declarations: [
    ScheduleOptionsComponent,
    NewEventResponsableComponent,
    NewEventRouteComponent,
    UpdateEventRouteComponent,
    UpdateEventResponsableComponent
  ],
  exports: [
    ScheduleOptionsComponent,
    NewEventResponsableComponent,
    NewEventRouteComponent,
    UpdateEventRouteComponent,
    UpdateEventResponsableComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    PipesModule
  ]
})
export class ComponentsConfigurationModule { }
