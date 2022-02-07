import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { MapComponent } from './map/map.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../Shared/shared.module';
import { AgmCoreModule } from '@agm/core';
import { environment } from 'src/environments/environment';



@NgModule({
  declarations: [
  
    HomeComponent,
    MapComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,   
    AgmCoreModule.forRoot({
      apiKey: environment.apiKey,
      libraries: environment.libraries
    })
    
  ]
})
export class HomeModule { }
