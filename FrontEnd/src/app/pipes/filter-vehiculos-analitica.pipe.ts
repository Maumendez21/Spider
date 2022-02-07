import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterVehiculosAnalitica'
})
export class FilterVehiculosAnaliticaPipe implements PipeTransform {

  transform(data: any, subempresa: string): any {

    if (subempresa != "") {
      data = data.filter(marker => (marker.Hierarchy == subempresa));
    }

    return data;
  }

}
