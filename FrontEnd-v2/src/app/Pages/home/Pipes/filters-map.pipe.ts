import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filtersMap'
})
export class FiltersMapPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }

}
