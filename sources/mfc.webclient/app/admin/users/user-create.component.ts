import { Component, Output, EventEmitter, ElementRef, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";

import { User } from './user.model';
import { UserService } from './user.service';

@Component({
    selector: 'mfc-user-create',
    templateUrl: 'app/admin/users/user-create.component.html'
})

export class UserCreateComponent {
    @Output() created: EventEmitter<User>;
    user: User;

    constructor(private userService: UserService) {
        this.created = new EventEmitter<User>();
        this.user = new User(0,'', '');
    }

    createNewUser(form: NgForm) {
        let user: User = this.user;
        this.created.emit(user);
        form.reset();
    }
}