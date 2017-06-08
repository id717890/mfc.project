import { Component } from '@angular/core';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { OrganizationService } from './organization.service';
import { OrganizationEditComponent } from './organization-edit.component';
import { DialogService } from '../../infrastructure/dialog/dialog.service';
import { Organization } from './../../models/organization.model';
import { OrganizationType } from './../../models/organization-type.model';
import { MdDialog, MdButton, MdDialogRef } from '@angular/material';

@Component({
    selector: 'mfc-organization-list',
    templateUrl: 'app/admin/organizations/organization-list.component.html'
})

export class OrganizationListComponent extends BaseListComponent<Organization> {
    constructor(public dialog: MdDialog, protected organizationService: OrganizationService, protected dialogService: DialogService) {
        super(dialog, organizationService, dialogService);// Перевести на material dialog + base component
    }

    newModel(): Organization {
        return new Organization(null, '', '', null);
    };

    cloneModel(model: Organization): Organization {
        return new Organization(model.id, model.caption, model.full_caption, model.organization_type);
    };

    getEditComponent(): any {
        return OrganizationEditComponent;
    }
}