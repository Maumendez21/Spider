import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RutesComponent } from './rutes/rutes.component';
import { PoinstInterestComponent } from './poinst-interest/poinst-interest.component';
import { ItinerariesResponsablesComponent } from './itineraries-responsables/itineraries-responsables.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { ComponentsModule } from '../../Components/components.module';
import { ComponentsMobilityModule } from './components/components-mobility.module';
import { NewRouteComponent } from './rutes/new-route/new-route.component';
import { RouterModule } from '@angular/router';
import { AgmCoreModule } from '@agm/core';
import { environment } from '../../../environments/environment.prod';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../Shared/shared.module';
import { UpdateRouteComponent } from './rutes/update-route/update-route.component';
import { FullCalendarModule } from '@fullcalendar/angular';

import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';
import { ConsultPointsInterestComponent } from './poinst-interest/consult-points-interest/consult-points-interest.component';
import { ActionPointInterestComponent } from './poinst-interest/action-point-interest/action-point-interest.component';
import { PointVinculationComponent } from './point-vinculation/point-vinculation.component';
import { PointDesvinculationComponent } from './point-desvinculation/point-desvinculation.component';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';

FullCalendarModule.registerPlugins([ // register FullCalendar plugins
  dayGridPlugin, interactionPlugin, timeGridPlugin
]);


@NgModule({
  declarations: [
    RutesComponent,
    PoinstInterestComponent,
    ItinerariesResponsablesComponent,
    ScheduleComponent,
    NewRouteComponent,
    UpdateRouteComponent,
    ConsultPointsInterestComponent,
    ActionPointInterestComponent,
    PointVinculationComponent,
    PointDesvinculationComponent,
  ],
  imports: [
    CommonModule,
    ComponentsModule,
    ComponentsMobilityModule,
    RouterModule,
    FullCalendarModule,
    FormsModule,
    SharedModule,
    AngularMultiSelectModule,
    AgmCoreModule.forRoot({
      apiKey: environment.apiKey,
      libraries: environment.libraries
    }),
  ]
})
export class MobilityModule { }
