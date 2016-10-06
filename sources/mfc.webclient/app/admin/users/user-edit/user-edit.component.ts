import { Component, Output, Input, EventEmitter } from "@angular/core";

import { User } from '../user.model';
import { UserService } from '../user.service'

@Component({
    selector: 'mfc-user-edit',
    templateUrl: 'app/admin/users/user-edit/user-edit.component.html'
})

export class UserEditComponent {
    @Output() edited: EventEmitter<User>;
    @Input() user: User;

    constructor(private userService: UserService) {
        this.edited = new EventEmitter<User>();
        this.user = new User('test', 'test');
    }

    editeUser() {
        let user: User = this.user;
        this.edited.emit(user);
    }
}