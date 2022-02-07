import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { HeaderComponent } from './header/header.component';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PipesModule } from '../Pipes/pipes.module';



@NgModule({
  declarations: [
    SidebarComponent,
    HeaderComponent,
    BreadcrumbComponent
  ],
  exports: [
    SidebarComponent,
    HeaderComponent,
    BreadcrumbComponent
    
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    PipesModule
  ]
})
export class SharedModule { }
