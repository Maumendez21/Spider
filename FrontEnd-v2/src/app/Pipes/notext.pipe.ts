import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'notext'
})
export class NotextPipe implements PipeTransform {

  // transform(value: unknown, ...args: unknown[]): unknown {
  //   return null;
  // }

  transform(text: string): string {
    if (text != null && text != "" && text != "0") {
      return text;
    } else {
      return 'N/D';
    }
  }

}
