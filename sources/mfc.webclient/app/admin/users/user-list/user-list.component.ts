import { Component, Output, EventEmitter } from '@angular/core';

import { UserCreateComponent  } from '../user-create/user-create.component';
import { UserEditComponent  } from '../user-edit/user-edit.component';
import { UserService  } from '../user.service';

import { User } from '../user.model';
import { Password } from '../password.model';

declare var jQuery: any; //объявляем переменную для возможности вызова jQuery

@Component({
    selector: 'mfc-user-list',
    templateUrl: 'app/admin/users/user-list/user-list.component.html'
})

export class UserListComponent {
    users: User[];
    selectedUser: User;
    selectedUserIndex: number;
    constructor(private userService: UserService) {
        userService.getUsers().then(user_list => this.users = user_list);
        this.selectedUser = new User(0, '', '');
    }

    selectUser(index: number) {
        this.selectedUserIndex = index;
        let user = new User();
        Object.assign(user,this.users[index]);
        this.selectedUser = user;
    }

    onPasswordChanged(password: Password): void {
        if (this.userService.changePassword(this.selectedUser, password)) {
            jQuery("#ChangePassword").modal("hide");
            alert('Пароль пользователя "' + this.selectedUser.user_name + ' | ' + this.selectedUser.description + '" успешно изменен');
        }
        else {
            alert('Ошибка сохранения');
        }
    }

    onUserCreated(user: User): void {
        this.userService.createUser(user).then((user) => {
            if (user != null) {
                jQuery("#CreateUser").modal("hide");
                this.users.push(user)
            }
        });
    }

    onUserEdited(user: User): void {
        if (this.userService.updateUser(user)) {
            this.refreshUser(this.selectedUserIndex, user);
            jQuery("#UpdateUser").modal("hide");
        }
        else {
            alert('Ошибка сохранения');
        }
    }

    deleteUser(index: number): void {
        if (confirm('Удалить пользователя "' + this.users[index].user_name + ' | ' + this.users[index].description + '" ?')) {
            let user = this.users[index];
            if (this.userService.deleteUser(user)) {
                this.users.splice(index, 1);
            } else {
                alert('Ошибка при удалении пользователя')
            }
        }
    }

    private addUser(user: User) {
        this.users.push(user);
    }

    private refreshUser(index: number, user: User) {
        Object.assign(this.users[index],user);
    }

    private refreshUserList(): void {
        this.userService.getUsers().then(user_list => this.users = user_list);
    }
}