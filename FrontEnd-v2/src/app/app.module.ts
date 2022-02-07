import { LOCALE_ID, NgModule } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import localeEs from '@angular/common/locales/es';
import { RouterModule } from '@angular/router';
registerLocaleData(localeEs);
// created
// import { SharedModule } from './Shared/shared.module';

import { AppComponent } from './app.component';
import { NoPageFoundComponent } from './no-page-found/no-page-found.component';
import { PagesModule } from './Pages/pages.module';
import { HttpClientModule } from '@angular/common/http';
import { AuthModule } from './Auth/auth.module';
import { DatePipe } from '@angular/common';
import { GoogleMapsAPIWrapper, PolygonManager } from '@agm/core';
import { JwPaginationModule } from 'jw-angular-pagination';


@NgModule({
  declarations: [
    AppComponent,
    NoPageFoundComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    PagesModule,
    AuthModule,
    JwPaginationModule,
  ],
  providers: [
    //AuthGuardService,
    DatePipe,
    GoogleMapsAPIWrapper,
    PolygonManager,
    // FilterSidebarRightPipe,
    {
      provide: LOCALE_ID,
      useValue: 'es'
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
