import { Component, Output, Input, EventEmitter } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { User } from '../../models/user.model';
import { UserService } from './user.service'
import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';
import { FormBuilder, Validators } from '@angular/forms';

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
    providers: [Modal]
})

export class UserEditComponent extends BaseEditComponent<User> {
    constructor(public dialog: DialogRef<BaseEditContext<User>>, formBuilder: FormBuilder) {
        super(dialog, formBuilder.group(
            {
                'user_name': [null, Validators.required],
                'description': [null, Validators.required],
                'is_admin': [false],
                'is_expert': [false],
                'is_controller': [false]
            }));
        this.formGroup.patchValue({
            user_name: dialog.context.model.user_name,
            description: dialog.context.model.description,
            is_admin: dialog.context.model.is_admin,
            is_expert: dialog.context.model.is_expert,
            is_controller: dialog.context.model.is_controller
        });
    }

    mapFormToModel(form: any): void {
        this.context.model.user_name = form.user_name;
        this.context.model.description = form.description;
        this.context.model.is_admin = form.is_admin;
        this.context.model.is_expert = form.is_expert;
        this.context.model.is_controller = form.is_controller;
    }
}