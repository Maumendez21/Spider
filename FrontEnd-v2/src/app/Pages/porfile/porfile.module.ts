import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangeLogoComponent } from './change-logo/change-logo.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    ChangeLogoComponent,
    ChangePasswordComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ]
})
export class PorfileModule { }
