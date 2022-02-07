import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GraphRendimientoComponent } from './graph-rendimiento/graph-rendimiento.component';
import { ChartsModule } from 'ng2-charts';
import { RankingTablesComponent } from './ranking-tables/ranking-tables.component';



@NgModule({
  declarations: [
    GraphRendimientoComponent,
    RankingTablesComponent
  ],
  exports: [
    GraphRendimientoComponent,
    RankingTablesComponent

  ],
  imports: [
    CommonModule,
    ChartsModule
  ]
})
export class ComponentsAnaliticsModule { }
