import { Component, Output, Input, EventEmitter, OnInit, AfterViewInit } from "@angular/core";
import { FormBuilder, Validators } from '@angular/forms';

// import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
// import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { OrganizationTypeService } from './../organization-types/organization-type.service';
import { OrganizationService } from './organization.service';
import { Organization } from './../../models/organization.model';
import { OrganizationType } from './../../models/organization-type.model';
import { OrganizationEditContext } from './organization-edit-context';

import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';

@Component({
    selector: 'modal-content',
    styles: [`
        .input-line {
            margin-bottom: 25px;
            width: 100%;
        }
        .input-buttons {
            margin-top: 10px;
        }
    `],
    templateUrl: 'app/admin/organizations/organization-edit.component.html'
    //,providers: [ Modal ]
})

export class OrganizationEditComponent extends BaseEditComponent<Organization> implements AfterViewInit {
    constructor(private organizationTypeService: OrganizationTypeService) {
        super();
    }

    ngAfterViewInit() {
        
    }
}