import { Directive, ElementRef, HostBinding, HostListener, Input, OnInit, Renderer2 } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Directive({
  selector: '[formFieldInvalid]'
})
export class FormFieldInvalidDirective implements OnInit {
  @Input('formFieldInvalid') fieldFormGroup: FormGroup;
  @Input() fieldName;

  constructor() {
  }

  ngOnInit(): void {
  }

  @HostBinding('class.error')
  public get isValid(): boolean {
    let field = this.fieldFormGroup.get(this.fieldName);
    return (field.invalid && (field.dirty || field.touched));
  }

}
