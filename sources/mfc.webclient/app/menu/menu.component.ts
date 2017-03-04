import { Component, Injectable, OnInit } from '@angular/core';

import { Messages } from './../infrastructure/application-messages';
import { ActionPermissionService } from './../infrastructure/security/action-permission.service';
import { HashTable } from './../shared/hash-table';

import {User} from './../models/user.model';

@Component({
    selector: 'main-menu',
    templateUrl: 'app/menu/menu.component.html'
})

@Injectable()
export class MenuComponent implements OnInit {
    actions: HashTable<boolean> = {};
    busy: Promise<any>;
    busyMessage: string;

    constructor(private actionPermissionService: ActionPermissionService) {
    }

    ngOnInit(): void {
        this.busyMessage = Messages.LOADING;
        this.busy = this.actionPermissionService.get("")
            .then(actions => {
                this.actions = {};
                for (var action of actions) {
                    this.actions[action] = true;
                }
            });
    }

    isLoggedIn(): boolean {
        return true;
    }

    getCurrentUser(): User {
        return new User(1, "Администратор", "admin", "", true, false, false);
    }
}