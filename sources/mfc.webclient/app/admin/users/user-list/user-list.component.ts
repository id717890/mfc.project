import { Component, Output, EventEmitter } from '@angular/core';

import { UserCreateComponent  } from '../user-create/user-create.component';
import { UserEditComponent  } from '../user-edit/user-edit.component';

import { UserService  } from '../user.service'
import { User } from '../user.model'

@Component({
    selector: 'mfc-user-list',
    templateUrl: 'app/admin/users/user-list/user-list.component.html'
})

export class UserListComponent {
    users: User[];
    selectedUser: User;
    constructor(private userService: UserService) {
        userService.getUsers().then(user_list => this.users = user_list);
        this.selectedUser = new User('', '');
    }

    selectUser(index: number) {
        this.selectedUser = this.users[index];
    }

    onUserCreated(user: User): void {
        this.userService.createUser(user).then(() => 
        {
            this.refreshUserList();
            alert('Пользователь успешно добавлен');
        });
    }

    onUserEdited(user: User): void {
        this.userService.updateUser(user)
        this.userService.updateUser(user).then(() => {
            this.refreshUserList();
            alert('Пользователь ' + user.user_name + ' | ' + user.description + ' успешно изменен');
        });
    }

    deleteUser(index: number): void {
       //Здесь надо вызывать диалог подтверждения
       let user=this.users[index];
        this.userService.deleteUser(user).then(() =>
        {
            this.refreshUserList();
            alert('Пользователь ' + user.user_name + ' | ' + user.description + ' успешно удален');
        });
    }

    private addUser(user: User) {
        this.users.push(user);
    }

    private refreshUserList(): void {
        this.userService.getUsers().then(user_list => this.users = user_list);
    }
}