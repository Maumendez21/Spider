import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterAnalitic'
})
export class FilterAnaliticPipe implements PipeTransform {

  transform(data: any, subempresa: string): any {

    if (subempresa != "") {
      data = data.filter(marker => (marker.Hierarchy == subempresa));
    }

    return data;
  }

}
