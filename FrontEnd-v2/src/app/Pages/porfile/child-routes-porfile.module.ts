import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ChangeLogoComponent } from './change-logo/change-logo.component';
import { ChangePasswordComponent } from './change-password/change-password.component';



const childRoutes: Routes = [
  { path: 'change-logo', component: ChangeLogoComponent,  data: { title: 'Cambiar logotipo' }},
  { path: 'change-password', component: ChangePasswordComponent,  data: { title: 'Cambiar Contraseña' }},
]


@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class ChildRoutesPorfileModule { }
