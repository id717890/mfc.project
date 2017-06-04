import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { BaseService } from './../../infrastructure/base.component/base.service';
import { CustomerType } from '../../models/customer-type.model';

@Injectable()
export class CustomerTypeService extends BaseService<CustomerType> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'customer-types';
    }
}