import { Component, Output, Input, EventEmitter, Inject } from "@angular/core";
import { NgForm } from "@angular/forms";

import { OrganizationType } from '../../models/organization-type.model';
import { OrganizationTypeService } from './organization-type.service';
import { MdDialog, MdButton, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'org-type-create-dlg',
    templateUrl: 'app/admin/organization-types/organization-type-create.component.html'
})

export class OrganizationTypeCreateComponent {
    header_text="";

    constructor(
        @Inject(MD_DIALOG_DATA) public data: OrganizationType,
        public dialogRef: MdDialogRef<OrganizationTypeCreateComponent>,
        private organization_type_service: OrganizationTypeService
    ) {
        console.log(data);
        if (data.id==null) this.header_text="Новый тип ОГВ"; else this.header_text="Редактирование"
    }

    submit(model: OrganizationType) {
        console.log(model);
    }
}