import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';

import { ActionType } from '../../models/action-type.model';
import { BaseService } from '../../infrastructure/base.component/base.service';

@Injectable()
export class ActionTypeService extends BaseService<ActionType> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'action-types';
    }
}