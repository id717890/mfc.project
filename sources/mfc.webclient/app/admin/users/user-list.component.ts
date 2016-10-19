import { Component } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';

import { UserEditComponent  } from './user-edit.component';
import { UserService  } from './user.service';

import { User } from './user.model';
import { Password } from './password.model';

@Component({
    selector: 'mfc-user-list',
    templateUrl: 'app/admin/users/user-list.component.html',
    providers: [Modal]
})

export class UserListComponent extends BaseListComponent<User> {
    busy: Promise<any>;
    busyMessage: string;
    users: User[];

    constructor(public modal: Modal, private userService: UserService) {
        super(modal, userService);
    }

    newModel(): User {
        return new User(null, '','','',false,false,false);
    };

    cloneModel(model: User): User {
        return new User(model.id, model.caption,model.user_name,model.description,model.is_admin,model.is_expert,model.is_controller);
    };

    getEditComponent(): any {
        return UserEditComponent;
    }
}