import { Component } from '@angular/core';

// import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';

import { UserEditComponent } from './user-edit.component';
import { UserService } from './user.service';

import { User } from '../../models/user.model';
import { Password } from '../../models/password.model';

@Component({
    selector: 'mfc-user-list',
    templateUrl: 'app/admin/users/user-list.component.html',
    styles: [`
        .glyphicon-ok-sign { color: green; font-size: 1.1em; }
        .glyphicon-minus-sign { color: silver }
    `],
})

export class UserListComponent extends BaseListComponent<User> {
    busy: Promise<any>;
    busyMessage: string;
    users: User[];

    constructor(private userService: UserService) {
        super(null, userService, null);// Перевести на material dialog + base component
    }

    newModel(): User {
        return new User(null, '', '', '', false, false, false);
    };

    cloneModel(model: User): User {
        return new User(model.id, model.caption, model.user_name, model.description, model.is_admin, model.is_expert, model.is_controller);
    };

    getEditComponent(): any {
        return UserEditComponent;
    }
}