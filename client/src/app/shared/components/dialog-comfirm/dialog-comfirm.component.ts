import { animate, style, transition, trigger } from '@angular/animations';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RotatedAnimation } from '../../animations/animation-triggers';


@Component({
  selector: 'app-dialog-comfirm',
  templateUrl: './dialog-comfirm.component.html',
  styleUrls: ['./dialog-comfirm.component.scss'],

})
export class DialogComfirm implements OnInit {

  constructor(private dialogRef: MatDialogRef<DialogComfirm>) { }

  ngOnInit(): void {
    this.dialogRef.disableClose = true;
  }

}
