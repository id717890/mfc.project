import { Component, Inject } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ActionType } from '../../models/action-type.model'
import { MaterialModule, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { BaseEditComponent } from './../../infrastructure/base.component/base-edit.component';
import { BaseContext } from './../../infrastructure/base.component/base-context.component';

@Component({
    selector: 'action-type-create-edit-dlg',
    templateUrl: 'app/admin/action-types/action-type.edit.component.html'
})

export class ActionTypeEditComponent extends BaseEditComponent<ActionType> {
    constructor(
        @Inject(MD_DIALOG_DATA) data: BaseContext<ActionType>,
        public dialogRef: MdDialogRef<ActionTypeEditComponent>
    ) {
        super(data, dialogRef);
    }
}