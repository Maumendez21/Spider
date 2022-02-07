import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { InspectionComponent } from './inspection/inspection.component';
import { UpdateInspectionComponent } from './inspection/update-inspection/update-inspection.component';
import { NewInspectionComponent } from './inspection/new-inspection/new-inspection.component';

const childRoutes: Routes = [
  { path: 'inspection', component: InspectionComponent, data: { title: 'Inspecciones' } },
  { path: 'inspection/:id', component: UpdateInspectionComponent, data: { title: 'Actualizar Inspección' } },
  { path: 'new-inspection', component: NewInspectionComponent, data: { title: 'Nueva Inspección' } },

  //{ path: 'path/:routeParam', component: MyComponent },
  //{ path: 'staticPath', component: ... },
  //{ path: '**', component: ... },
  //{ path: 'oldPath', redirectTo: '/staticPath' },
  //{ path: ..., component: ..., data: { message: 'Custom' }
];


@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class ChildRoutesMaitananceModule { }
