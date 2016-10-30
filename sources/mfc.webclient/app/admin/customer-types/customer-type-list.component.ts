import { Component } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { OnInit } from '@angular/core';
import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';

import { CustomerTypeService } from './customer-type.service';
import { CustomerType } from '../../models/customer-type.model';
import { CustomerTypeEditComponent } from './customer-type-edit.component';

@Component({
    selector: 'mfc-customer-type-list',
    templateUrl: 'app/admin/customer-types/customer-type-list.component.html',
    providers: [Modal]
})

export class CustomerTypeListComponent extends BaseListComponent<CustomerType> implements OnInit {
    customerTypes: CustomerType[];
    busy: Promise<any>;
    busyMessage: string;


    constructor(public modal: Modal, private customerTypeService: CustomerTypeService) {
        super(modal, customerTypeService);
    }

    newModel(): CustomerType {
        return new CustomerType(null, '');
    };

    cloneModel(model: CustomerType): CustomerType {
        return new CustomerType(model.id, model.caption);
    };

    getEditComponent(): any {
        return CustomerTypeEditComponent;
    }
}