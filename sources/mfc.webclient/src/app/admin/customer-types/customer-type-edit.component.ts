import { Component, Output, Input, EventEmitter } from "@angular/core";
import { FormBuilder, Validators } from '@angular/forms';

//import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
//import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { CustomerTypeService } from './customer-type.service';
import { CustomerType } from './../../models/customer-type.model';
import { BaseContext } from './../../infrastructure/base.component/base-context.component';
import { BaseEditComponent } from './../../infrastructure/base.component/base-edit.component';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/admin/customer-types/customer-type-edit.component.html'/*,
    providers: [ Modal ]*/
})

export class CustomerTypeEditComponent extends BaseEditComponent<CustomerType> {
    /*constructor(public dialog: DialogRef<BaseEditContext<CustomerType>>) {
        super(dialog);
    }*/
}