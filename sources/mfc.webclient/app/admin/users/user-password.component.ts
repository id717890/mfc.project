import { Component, Output, Input, EventEmitter } from "@angular/core";
import { NgForm } from "@angular/forms";

import { User } from './user.model';
import { Password } from './password.model';
import { UserService } from './user.service';

@Component({
    selector: 'mfc-user-password',
    templateUrl: 'app/admin/users/user-password.component.html'
})

export class UserPasswordComponent {
    @Output() changedPassword: EventEmitter<Password>;
    @Input() user: User;
    pass: Password;

    constructor(private userService: UserService) {
        this.changedPassword = new EventEmitter<Password>();
        this.user = new User(0, 'test', 'test');
        this.pass = new Password('', '');
    }

    changePassword(form: NgForm) {
        if (this.pass.password != this.pass.confirm) {
            alert('Пароль и подтверждение пароля не совпадают!')
        }
        else {
            this.changedPassword.emit(this.pass);
            form.reset();
        }
    }
}