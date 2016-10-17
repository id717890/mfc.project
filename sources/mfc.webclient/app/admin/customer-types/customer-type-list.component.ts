import { Component } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {OnInit} from '@angular/core';
import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import {CustomerTypeService} from './customer-type.service';
import {CustomerType} from './customer-type';
import {CustomerTypeEditComponent} from './customer-type-edit.component';
import {SAVE_MESAGE, LOAD_LIST_MESAGE} from './../../infrastructure/application-messages';

@Component({
    selector: 'mfc-customer-type-list',
    templateUrl: 'app/admin/customer-types/customer-type-list.component.html',
    providers: [Modal]
})

export class CustomerTypeListComponent implements OnInit {
    customerTypes: CustomerType[];
    busy: Promise<any>;
    busyMessage: string;


    constructor(public modal: Modal, private customerTypeService: CustomerTypeService) {
    }

    ngOnInit(): void {
        this.fillCustomerTypes();
    }

    addType() {
        this.modal
            .open(
            CustomerTypeEditComponent,
            overlayConfigFactory({ title: 'Новый', customerType: new CustomerType(null, '') }, BSModalContext)
            ).then(x => {
                x.result.then(output => {
                    if (output != null) {
                        this.busyMessage = SAVE_MESAGE;
                        this.busy = this.customerTypeService.addCustomerType(output)
                            .then(x => {
                                this.customerTypes.push(x);
                            }).catch(x => this.handlerError(x));
                    }
                }, () => null);
            }).catch(this.handlerError);
    }

    editType(type: CustomerType) {
        this.modal
            .open(
            CustomerTypeEditComponent,
            overlayConfigFactory({ title: 'Редактирование', customerType: new CustomerType(type.id, type.caption) }, BSModalContext)
            ).then(x => {
                x.result.then(output => {
                    if (output != null) {
                        this.busyMessage = SAVE_MESAGE;
                        this.busy = this.customerTypeService.updateCustomerType(output)
                            .then(x => {
                                type.caption = output.caption;
                            }).catch(x => this.handlerError(x));
                    }
                }, () => null);
            }).catch(this.handlerError);
    }

    deleteType(type: CustomerType) {
        this.busyMessage = SAVE_MESAGE;
        this.busy = this.customerTypeService.deleteCustomerType(type)
            .then(res => {
                if (res) {
                    this.customerTypes.splice(this.customerTypes.indexOf(type), 1);
                }
            });
    }

    private fillCustomerTypes() {
        this.busyMessage = LOAD_LIST_MESAGE;
        this.busy = this.customerTypeService.getCustomerTypes()
            .then(types => this.customerTypes = types);
    }

    private handlerError(error: any) {
        console.log('CustomerTypeListComponent::error ' + error);
    }
}