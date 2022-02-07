import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotextPipe } from './notext.pipe';
import { FilterSidebarRightPipe } from './filter-sidebar-right.pipe';
import { FilterAnaliticPipe } from './filter-analitic.pipe';
import { CountArrayPipe } from './count-array.pipe';



@NgModule({
  declarations: [
    NotextPipe,
    FilterSidebarRightPipe,
    FilterAnaliticPipe,
    CountArrayPipe,
  ],
  exports: [
    NotextPipe,
    FilterSidebarRightPipe,
    FilterAnaliticPipe,
    CountArrayPipe,
  ],
  providers: [
    FilterSidebarRightPipe,

  ],
  imports: [
    CommonModule
  ]
})
export class PipesModule { }
