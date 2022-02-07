import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VehiculosActivesComponent } from './vehiculos-actives/vehiculos-actives.component';
import { ComponentsModule } from '../../../Components/components.module';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    VehiculosActivesComponent
  ],
  exports: [
    VehiculosActivesComponent
  ],
  imports: [
    CommonModule,
    ComponentsModule,
    FormsModule
  ]
})
export class AnaliticSharedModule { }
