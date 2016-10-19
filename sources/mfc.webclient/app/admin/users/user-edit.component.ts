import { Component, Output, Input, EventEmitter } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { User } from './user.model';
import { UserService } from './user.service'
import {BaseEditComponent, BaseEditContext} from './../../infrastructure/base.component/base-edit.component';

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
    templateUrl: 'app/admin/users/user-edit.component.html',
    providers: [ Modal ]
})

export class UserEditComponent extends BaseEditComponent<User> {
    constructor(public dialog: DialogRef<BaseEditContext<User>>) {
        super(dialog);
    }
}