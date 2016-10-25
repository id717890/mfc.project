//import { Component, Output, EventEmitter } from '@angular/core';
//import {OnInit} from '@angular/core';

import { Component } from '@angular/core';
import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../../infrastructure/base.component/base-list.component';

import { OrganizationTypeService } from '../organization-type.service';
import { OrganizationTypeEditComponent } from '../organization-type-edit.component'

import { OrganizationType } from '../../../models/organization-type.model';

@Component({
    selector: 'mfc-organizationType-list',
    templateUrl: 'app/admin/organization-types/organization-type-list/organization-type-list.component.html',
    providers: [Modal]
})

export class OrganizationTypeListComponent extends BaseListComponent<OrganizationType> {
    //organizationTypes: OrganizationType[] = [];
    //selectedOrganizationType: OrganizationType;
    busy: Promise<any>;
    busyMessage: string;
	
    constructor(public modal: Modal, private organizationTypeService: OrganizationTypeService){
        super(modal, organizationTypeService);
        //this.refreshList();
    }

    //private refreshList(): void {
    //    this.organizationTypeService.get().then(orientation_types_list => this.organizationTypes = orientation_types_list);
    //}

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