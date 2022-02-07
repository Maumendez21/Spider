import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'notext'
})
export class NotextPipe implements PipeTransform {

  transform(text: string): string {
    if (text != null && text != "" && text != "0") {
      return text;
    } else {
      return 'N/D';
    }
  }

}
