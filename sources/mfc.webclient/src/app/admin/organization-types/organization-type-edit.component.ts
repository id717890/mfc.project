import { Component, Output, Input, EventEmitter, Inject } from "@angular/core";
import { NgForm } from "@angular/forms";

import { OrganizationType } from '../../models/organization-type.model';
import { OrganizationTypeService } from './organization-type.service';
import { OrganizationTypeContext } from './organization-type.context';
import { MaterialModule, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'org-type-create-edit-dlg',
    templateUrl: 'app/admin/organization-types/organization-type-edit.component.html'
})

export class OrganizationTypeEditComponent {
    public header_text = "";
    public context:OrganizationTypeContext;

    constructor(
        @Inject(MD_DIALOG_DATA) public data: OrganizationTypeContext,
        public dialogRef: MdDialogRef<OrganizationTypeEditComponent>
    ) {
        this.context=data;
        if (data.organization_type.id == null) this.header_text = "Новый тип ОГВ"; else this.header_text = "Редактирование"
    }

    submit(model: OrganizationType) {
        this.dialogRef.close(model)
    }
}