import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormFieldService {

  constructor() { }

  public field_error(formGroup: FormGroup, field_name: string, valid_name: string) {
    let field = formGroup.get(field_name);
    return field.hasError(valid_name) && (field.dirty || field.touched);
  }
}
