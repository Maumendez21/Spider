import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterSidebarRight'
})
export class FilterSidebarRightPipe implements PipeTransform {

  transform(data: any, subempresa: string, estatus: string, type: string): any {

    if (subempresa != "") {
      data = data.filter(marker => (marker.nombreEmpresa == subempresa));
    }

    if (estatus != "") {
      data = data.filter(marker => (marker.statusEvent == estatus));
    }
    if (type != "") {
      switch (type) {
        case "1":
          data = data.filter(marker => (marker.typeDevice == type || marker.typeDevice == '2' || marker.typeDevice == '3' ));
          break;
        case "5":
          data = data.filter(marker => (marker.typeDevice == type || marker.typeDevice == '7'));
          break;

        default:
          data = data.filter(marker => (marker.typeDevice == type));
          break;
      }
    }

    return data;
  }
}
