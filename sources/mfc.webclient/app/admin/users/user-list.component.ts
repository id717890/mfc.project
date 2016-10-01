import { Component } from '@angular/core';
@Component({
    selector: 'mfc-user-list',
    templateUrl: 'app/admin/users/user-list.component.html'
})
export class UserListComponent {
    users:any[]=[
        {'id':'1','user_name':'Test1', 'description':'Test2','is_admin':true,'is_controller':false,'is_expert':false},
        {'id':'2','user_name':'Test2', 'description':'Test2','is_admin':true,'is_controller':false,'is_expert':false},
        {'id':'3','user_name':'Test1', 'description':'Test2','is_admin':true,'is_controller':false,'is_expert':false},
        {'id':'4','user_name':'Test1', 'description':'Test2','is_admin':true,'is_controller':false,'is_expert':false}
    ]
}