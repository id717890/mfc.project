import { Component, Output, Input, EventEmitter } from "@angular/core";
import { NgForm } from "@angular/forms";
import { User } from './user.model';
import { UserService } from './user.service'

@Component({
    selector: 'mfc-user-edit',
    templateUrl: 'app/admin/users/user-edit.component.html'
})

export class UserEditComponent {
    @Output() edited: EventEmitter<User>;
    @Input() user: User;

    constructor(private userService: UserService) {
        this.edited = new EventEmitter<User>();
        this.user = new User(0, 'test', 'test');
    }

    editeUser(form: NgForm) {
        this.edited.emit(this.user);
        form.reset();
    }
}