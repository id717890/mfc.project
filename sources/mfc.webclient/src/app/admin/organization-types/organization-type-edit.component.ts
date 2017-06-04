import { Component, Output, Input, EventEmitter, Inject } from "@angular/core";
import { NgForm } from "@angular/forms";
import { OrganizationType } from '../../models/organization-type.model';
import { MaterialModule, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { BaseEdit2Component } from './../../infrastructure/base.component/base-edit2.component';
import { BaseContext } from './../../infrastructure/base.component/base-context.component';

@Component({
    selector: 'org-type-create-edit-dlg',
    templateUrl: 'app/admin/organization-types/organization-type-edit.component.html'
})

export class OrganizationTypeEditComponent extends BaseEdit2Component<OrganizationType> {
    constructor(
        @Inject(MD_DIALOG_DATA) data: BaseContext<OrganizationType>,
        public dialogRef: MdDialogRef<OrganizationTypeEditComponent>
    ) {
        super(data, dialogRef);
    }
}