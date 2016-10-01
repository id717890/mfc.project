import { Component } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { User, UserService } from './user.service'

@Component({
    selector: 'mfc-user-list',
    templateUrl: 'app/admin/users/user-list.component.html'
})

export class UserListComponent {
    users: User[];
    
    constructor(private userService: UserService) {
        userService.getUsers().then(user_list => this.users = user_list);
    }
}