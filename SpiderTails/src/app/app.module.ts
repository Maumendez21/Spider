import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { NoPageFoundComponent } from './no-page-found/no-page-found.component';
import { AppRoutingModule } from './app-routing.module';
import { DatePipe } from '@angular/common';
import { PagesModule } from './Pages/pages.module';
import { RouterModule } from '@angular/router';
import { AuthModule } from './Auth/auth.module';
import { GoogleMapsAPIWrapper, PolygonManager } from '@agm/core';

@NgModule({
  declarations: [
    AppComponent,
    NoPageFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    PagesModule,
    RouterModule,
    AuthModule
    
  ],
  providers: [
    DatePipe,
    GoogleMapsAPIWrapper,
    PolygonManager,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
