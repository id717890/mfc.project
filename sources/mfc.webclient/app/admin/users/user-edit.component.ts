import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { User } from '../../models/user.model';
import { UserService } from './user.service'
import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/admin/users/user-edit.component.html',
    providers: [Modal]
})

export class UserEditComponent extends BaseEditComponent<User> {
    constructor(public dialog: DialogRef<BaseEditContext<User>>) {
        super(dialog);
    }
}