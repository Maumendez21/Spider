import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoPageFoundComponent } from './no-page-found/no-page-found.component';
import { RouterModule, Routes } from '@angular/router';
import { PagesRoutingModule } from './Pages/pages.routing';
import { AuthRoutingModule } from './Auth/auth.routing';

const routes: Routes = [
  { path: '**', component: NoPageFoundComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {useHash: true}),
    PagesRoutingModule,
    AuthRoutingModule
  ]
})
export class AppRoutingModule { }
