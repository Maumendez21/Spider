import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalImportKMLComponent } from './modal-import-kml/modal-import-kml.component';
import { ModalImportPointComponent } from './modal-import-point/modal-import-point.component';



@NgModule({
  declarations: [
    ModalImportKMLComponent,
    ModalImportPointComponent
  ],
  exports: [
    ModalImportKMLComponent,
    ModalImportPointComponent
    
  ],
  imports: [
    CommonModule
  ]
})
export class ComponentsMobilityModule { }
