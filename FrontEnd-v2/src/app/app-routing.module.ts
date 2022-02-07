import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { NoPageFoundComponent } from './no-page-found/no-page-found.component';
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
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
