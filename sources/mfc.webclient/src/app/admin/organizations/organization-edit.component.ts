import { Component, Inject } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Organization } from './../../models/organization.model';
import { OrganizationType } from './../../models/organization-type.model';
import { OrganizationTypeService } from '../organization-types/organization-type.service';
import { MaterialModule, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { BaseEdit2Component } from './../../infrastructure/base.component/base-edit2.component';
import { BaseContext } from './../../infrastructure/base.component/base-context.component';

@Component({
    selector: 'ogv-create-edit-dlg',
    templateUrl: 'app/admin/organizations/organization-edit.component.html'
})

export class OrganizationEditComponent extends BaseEdit2Component<Organization>{
    organization_types: OrganizationType[];
    // selected_ogv: OrganizationType;
    busy: Promise<any>;
    busyMessage: string;

    constructor(
        @Inject(MD_DIALOG_DATA) data: BaseContext<Organization>,
        public dialogRef: MdDialogRef<OrganizationEditComponent>,
        private organization_type_service: OrganizationTypeService
    ) {
        super(data, dialogRef);
        this.fillLists();
    }

    private fillLists(): void {
        this.busy = this.organization_type_service.get().then(x => {
            let organization_types: OrganizationType[] = [OrganizationType.AllOrganizationType];
            organization_types = organization_types.concat(x.data);
            this.organization_types = organization_types;

            //Пока что такое решение, чтобы при редактировании в combobox подставлялся объект из списка
            this.context.model.organization_type = this.organization_types.filter(x => x.id == this.context.model.organization_type.id)[0];
        });
    }
}