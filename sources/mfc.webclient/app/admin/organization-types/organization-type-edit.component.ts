import { Component, Output, Input, EventEmitter } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';


import { OrganizationType } from '../../models/organization-type.model';
import { OrganizationTypeService } from './organization-type.service';
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
    templateUrl: 'app/admin/organization-types/organization-type-edit.component.html',
    providers: [ Modal ]
})

export class OrganizationTypeEditComponent extends BaseEditComponent<OrganizationType> {
    constructor(public dialog: DialogRef<BaseEditContext<OrganizationType>>) {
        super(dialog);
    }
}