import { Component, Output, EventEmitter, OnInit } from '@angular/core';

import { UserCreateComponent  } from '../user-create/user-create.component';
import { UserEditComponent  } from '../user-edit/user-edit.component';
import { UserService  } from '../user.service';

import { User } from '../user.model';
import { Password } from '../password.model';
import { LoadingIndicator, LoadingPage } from '../../../Infrastructure/loading-service/loading.component';

declare var jQuery: any; //объявляем переменную для возможности вызова jQuery

@Component({
    selector: 'mfc-user-list',
    templateUrl: 'app/admin/users/user-list/user-list.component.html'
})

export class UserListComponent extends LoadingPage {
    busy: Promise<any>;
    users: User[];
    selectedUser: User;
    selectedUserIndex: number;
    constructor(private userService: UserService) {
        super(false);
        this.selectedUser = new User(0, '', '');
    }

    ngOnInit() {
        this.busy = this.userService.getUsers().then(user_list => {
            this.users = user_list;
        });
    }

    selectUser(index: number) {
        this.selectedUserIndex = index;
        let user = new User();
        Object.assign(user, this.users[index]);
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
        this.standby();
        this.userService.createUser(user).then((user) => {
            if (user != null) {
                jQuery("#CreateUser").modal("hide");
                this.users.push(user)
            }
            this.ready();
        });
    }

    onUserEdited(user: User): void {
        this.standby();
        this.userService.updateUser(user).then(isOk => {
            if (isOk) {
                this.refreshUser(this.selectedUserIndex);
                jQuery("#UpdateUser").modal("hide");
            } else {
                alert('Ошибка сохранения');
                this.ready();
            }
        })
    }

    deleteUser(index: number): void {
        if (confirm('Удалить пользователя "' + this.users[index].user_name + ' | ' + this.users[index].description + '" ?')) {
            let user = this.users[index];
            this.standby();
            this.userService.deleteUser(user).then(isOk => {
                if (isOk) {
                    this.users.splice(index, 1);
                } else {
                    alert('Ошибка при удалении пользователя')
                }
                this.ready();
            })
        }
    }

    private addUser(user: User) {
        this.users.push(user);
    }

    private refreshUser(index: number) {
        this.standby();
        this.userService.getUserById(this.users[index].id).then(x => {
            Object.assign(this.users[index], x);
            this.ready();
        }, () => this.ready())
    }
}