import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AccountService } from 'src/app/account/account.service';
import { IAddress } from 'src/app/models/address';
import { IApiResponse } from 'src/app/models/apiResponse';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent implements OnInit {
  @Input() addressForm: FormGroup;

  constructor(
    public translate: TranslateService,
    private accountService: AccountService,
    private formBuilder: FormBuilder) { }


  ngOnInit(): void {
    this.validateLoginForm();
  }

  validateLoginForm() {

    this.addressForm = this.formBuilder.group({
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      street: [null, [Validators.required]],
      city: [null, [Validators.required]],
      state: [null, [Validators.required]],
      zipCode: [null, [Validators.required]],
    });
  }

  getTranslate(name: string) {

    let trans = this.translate.instant('CheckoutAddress');
    let result = { name: trans[name] }

    return result;
  }

  setUserAddress() {
    this.accountService.getUserAddress().subscribe((res: IApiResponse<IAddress>) => {
      if (res.data) {
        this.addressForm.reset(res.data);
      }
    }, error => {

    });
  }

  // saveUserAddress() {
  //   this.accountService.updateUserAddress(this.addressForm.value).subscribe((res: IApiResponse<IAddress>) => {
  //     console.log(this.addressForm.value);
  //     console.log(res.data);
  //     this.addressForm.reset(res.data);
  //   }, error => {

  //   });
  // }


}
