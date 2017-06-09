import { Component } from '@angular/core';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { OrganizationTypeService } from './organization-type.service';
import { OrganizationTypeEditComponent } from './organization-type-edit.component';
import { DialogService } from '../../infrastructure/dialog/dialog.service';
import { OrganizationType } from '../../models/organization-type.model';
import { MdDialog, MdButton, MdDialogRef } from '@angular/material';

@Component({
    selector: 'mfc-organization-type-list',
    templateUrl: 'app/admin/organization-types/organization-type-list.component.html'
})

export class OrganizationTypeListComponent extends BaseListComponent<OrganizationType> {
    constructor(public dialog: MdDialog, protected organizationTypeService: OrganizationTypeService, protected dialogService: DialogService) {
        super(dialog, organizationTypeService, dialogService);
    }

    newModel(): OrganizationType {
        return new OrganizationType(null, '');
    };

    cloneModel(model: OrganizationType): OrganizationType {
        return new OrganizationType(model.id, model.caption);
    };

    getEditComponent(): any {
        return OrganizationTypeEditComponent;
    }
}