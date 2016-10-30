import { Component, Output, Input, EventEmitter, OnInit, AfterViewInit } from "@angular/core";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

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
    templateUrl: 'app/admin/organizations/organization-edit.component.html',
    providers: [ Modal ]
})

export class OrganizationEditComponent extends BaseEditComponent<Organization> implements AfterViewInit {
    constructor(public dialog: DialogRef<OrganizationEditContext>, private organizationTypeService: OrganizationTypeService) {
        super(dialog);
    }

    ngAfterViewInit() {
        this.organizationTypeService.get()
            .then(models => {
                this.dialog.context.organizationTypes = models['data'];
                
                //поскольку в typescript нет возможности переопределить
                //оператор равенства, то для нормальной работы binding selector
                //заменяет ссылку на OrganizationType из формы списка на полученную в запросе 
                var types = this.dialog.context.organizationTypes;
                if (types != null && this.dialog.context.model.organization_type != null) {
                    var id = this.dialog.context.model.organization_type.id;
                    for (var item of types) {
                        if (item.id == id){
                            this.dialog.context.model.organization_type = item;
                            break; 
                        }
                    }
                }
            });
    }
}