import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'trucateText'
})
export class TrucateTextPipe implements PipeTransform {

  transform(value: string, maxLength: number = 100, sliceLength: number = 100): string {
    if (value.length > maxLength) {

      return `${value.slice(0, sliceLength)} ......`;
    }
    return value;
  }

}
