import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'countArray'
})
export class CountArrayPipe implements PipeTransform {

  transform(data: any[]): number {
    return data.length;
  }

}
