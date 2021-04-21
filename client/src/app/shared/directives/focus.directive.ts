import { Directive, ElementRef, Inject, Input, OnChanges, SimpleChanges } from '@angular/core';

@Directive({
  selector: '[focusField]'
})
export class FocusDirective implements OnChanges {
  @Input('focusField') focusField: any;

  constructor(@Inject(ElementRef) private element: ElementRef) { }
  ngOnChanges(changes: SimpleChanges): void {

    console.log(this.element.nativeElement.focus());
    if (changes.focusField.currentValue) {
      console.log('focus2')
      this.element.nativeElement.style.background = "red";
    }
  }

}
