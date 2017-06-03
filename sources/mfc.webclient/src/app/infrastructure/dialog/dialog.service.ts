import { Observable } from 'rxjs/Rx';
import { DialogConfirm } from './dialog-confirm.component';
import { DialogAlert } from './dialog-alert.component';
import { MdDialogRef, MdDialog, MdDialogConfig } from '@angular/material';
import { Injectable } from '@angular/core';

@Injectable()
export class DialogService {

    constructor(private dialog: MdDialog) { }

    public confirm(title: string, message: string): Observable<boolean> {

        let dialogRef: MdDialogRef<DialogConfirm>;

        dialogRef = this.dialog.open(DialogConfirm);
        dialogRef.componentInstance.title = title;
        dialogRef.componentInstance.message = message;

        return dialogRef.afterClosed();
    }

    public alert(title: string, message: string) {

        let dialogRef: MdDialogRef<DialogAlert>;

        dialogRef = this.dialog.open(DialogAlert);
        dialogRef.componentInstance.title = title;
        dialogRef.componentInstance.message = message;

        return dialogRef.afterClosed();
    }
}