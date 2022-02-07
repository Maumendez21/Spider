import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { NotificationsSharedComponent } from './notifications-shared/notifications-shared.component';
import { DataTablesModule } from 'angular-datatables';
import { AgmCoreModule } from '@agm/core';
import { environment } from '../../environments/environment.prod';
import { CardComponent } from './card/card.component';
import { JwPaginationCustomComponent } from './jw-pagination-custom/jw-pagination-custom.component';
import { JwPaginationModule } from 'jw-angular-pagination';
import { PipesModule } from '../Pipes/pipes.module';
import { FiltersFormComponent } from './filters-form/filters-form.component';
import { FormsModule } from '@angular/forms';
import { ModalReportComponent } from './modal-report/modal-report.component';
// import { JwPaginationModule } from 'jw-angular-pagination';



@NgModule({
  declarations: [
    NotificationsSharedComponent,
    CardComponent,
    JwPaginationCustomComponent,
    FiltersFormComponent,
    ModalReportComponent
  ],
  exports: [
    NotificationsSharedComponent,
    CardComponent,
    JwPaginationCustomComponent,
    FiltersFormComponent,
    ModalReportComponent
  ],
  imports: [
    CommonModule,
    DataTablesModule,
    JwPaginationModule,
    PipesModule,
    FormsModule,
    AgmCoreModule.forRoot({
      apiKey: environment.apiKey,
      libraries: environment.libraries
    })
  ],
  providers: [
    //AuthGuardService,
    DatePipe,
  ]
})
export class ComponentsModule { }
