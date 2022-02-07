import { Pipe, PipeTransform } from '@angular/core';
import { DictionaryEs } from '../i18n/es';

@Pipe({
  name: 'translate'
})
export class TranslatePipe implements PipeTransform {

  transform(text: string): string {

    /*if (navigator.language.substring(0, 2) == "es") {
      return DictionaryEs[text];
    } else {
      return DictionaryEn[text];
    }*/

    return DictionaryEs[text];
    
  }

}
