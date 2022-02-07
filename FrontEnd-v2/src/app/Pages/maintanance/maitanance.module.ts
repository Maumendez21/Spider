import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InspectionComponent } from './inspection/inspection.component';
import { Routes, RouterModule } from '@angular/router';
import { ComponentsModule } from '../../Components/components.module';
import { UpdateInspectionComponent } from './inspection/update-inspection/update-inspection.component';
import { NewInspectionComponent } from './inspection/new-inspection/new-inspection.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    InspectionComponent,
    UpdateInspectionComponent,
    NewInspectionComponent
  ],
  imports: [
    CommonModule,
    ComponentsModule,
    RouterModule,
    FormsModule
  ]
})
export class MaitananceModule { }
