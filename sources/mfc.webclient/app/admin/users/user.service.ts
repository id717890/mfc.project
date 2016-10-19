import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { User } from './user.model';
import { Password } from './password.model';
import { BaseService } from './../../infrastructure/base.component/base.service';

@Injectable()
export class UserService extends BaseService<User> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'user';
    }
}