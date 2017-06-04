import { MdDialogRef } from '@angular/material';
import { Component } from '@angular/core';

@Component({
    selector: 'dialog-confirm',
    template: `
        <p>{{ title }}</p>
        <p>{{ message }}</p>
        <button type="button" md-button (click)="dialogRef.close()">OK</button>
    `,
})
export class DialogAlert {

    public title: string;
    public message: string;

    constructor(public dialogRef: MdDialogRef<DialogAlert>) {

    }
}