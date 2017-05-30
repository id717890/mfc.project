//import { Component, Output, EventEmitter } from '@angular/core';
//import {OnInit} from '@angular/core';
// import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { Component } from '@angular/core';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';

import { OrganizationTypeService } from './organization-type.service';
import { OrganizationTypeEditComponent } from './organization-type-edit.component'
import { OrganizationTypeCreateComponent } from './organization-type-create.component'

import { OrganizationType } from '../../models/organization-type.model';

import { MdDialog, MdButton, MdDialogRef } from '@angular/material';

@Component({
    selector: 'mfc-organizationType-list',
    templateUrl: 'app/admin/organization-types/organization-type-list.component.html'
    //,providers: [Modal]
})

export class OrganizationTypeListComponent extends BaseListComponent<OrganizationType> {
    //organizationTypes: OrganizationType[] = [];
    //selectedOrganizationType: OrganizationType;
    busy: Promise<any>;
    busyMessage: string;



    constructor(public dialog: MdDialog, private organizationTypeService: OrganizationTypeService) {
        super(organizationTypeService);
        //this.refreshList();
    }

    create_ogv_type() {
        let dialogRef = this.dialog.open(OrganizationTypeCreateComponent, {
            data: new OrganizationType(null, '')
        });
    }

    edit2(model: OrganizationType) {
        let dialogRef = this.dialog.open(OrganizationTypeCreateComponent, {
            data: model
        });

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

@Component({
    selector: 'ttt',
    templateUrl: './test.html',
})
export class DialogOverviewExampleDialog { }