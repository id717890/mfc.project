import { Component, Output, Input, EventEmitter } from "@angular/core";
import { NgForm } from "@angular/forms";

// import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
// import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { User } from '../../models/user.model';
import { UserService } from './user.service'
import { BaseContext } from './../../infrastructure/base.component/base-context.component';
import { BaseEditComponent } from './../../infrastructure/base.component/base-edit.component';
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
})

export class UserEditComponent extends BaseEditComponent<User> {

    
}