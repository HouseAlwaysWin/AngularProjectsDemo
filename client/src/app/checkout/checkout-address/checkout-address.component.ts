import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatStepper } from '@angular/material/stepper';
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
  @Input() checkoutForm: FormGroup;

  get addressForm() {
    return this.checkoutForm.get('addressForm');
  }
  constructor(
    public translate: TranslateService,
    private accountService: AccountService
  ) { }


  ngOnInit(): void {
  }



  getTranslate(name: string) {

    let trans = this.translate.instant('CheckoutAddress');
    let result = { name: trans[name] }

    return result;
  }

  setUserAddress() {
    this.accountService.getUserAddress().subscribe((res: IApiResponse<IAddress>) => {
      if (res.data) {
        this.checkoutForm.get('addressForm').reset(res.data);
      }
    }, error => {

    });
  }

}
