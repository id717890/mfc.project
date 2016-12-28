import { Component, AfterViewInit } from "@angular/core";
import { FormBuilder } from '@angular/forms';

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { Service } from './../../models/service.model';
import { Organization } from './../../models/organization.model';
import { OrganizationService } from '../../admin/organizations/organization.service';

import {BaseEditComponent, BaseEditContext} from './../../infrastructure/base.component/base-edit.component';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/admin/services/service-edit.component.html',
    providers: [Modal]
})

export class ServiceEditComponent extends BaseEditComponent<Service> implements AfterViewInit {
    busy: Promise<any>;
    busyMessage: string = "Загрузка списков...";
    organizations: Organization[];

    constructor(public dialog: DialogRef<BaseEditContext<Service>>, private _organizationService: OrganizationService) {
        super(dialog);
    }

    ngAfterViewInit() {
        this.fillForm();
    }

    private fillForm() {
        //получаем список ОГВ для combobox, если действие редактирование заполняем поле
        this.busy =
            this._organizationService.get().then(x => {
                this.organizations = x["data"];
                if (this.organizations != null)
                    if (this.context.model.organization != null) {
                        var id = this.dialog.context.model.organization.id;
                        this.dialog.context.model.organization = this.organizations.filter(x => x.id == id)[0];
                    } else this.context.model.organization = this.organizations[0];
            });
    }
}