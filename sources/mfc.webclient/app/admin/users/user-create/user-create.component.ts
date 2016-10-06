import { Component, Output, EventEmitter } from "@angular/core";

import { User } from '../user.model';
import { UserService } from '../user.service'

@Component({
    selector: 'mfc-user-create',
    templateUrl: 'app/admin/users/user-create/user-create.component.html'
})

export class UserCreateComponent{
    @Output() created:EventEmitter<User>;

    user:User;

    constructor(private userService: UserService){
        this.created=new EventEmitter<User>();
        this.user=new User('','');               
    }    

    createNewUser()
    {
        let user:User=this.user;
        this.created.emit(user);
    }

}