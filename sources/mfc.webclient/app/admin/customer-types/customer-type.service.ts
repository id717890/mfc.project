import { Injectable }     from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import {BaseService} from './../../infrastructure/base.component/base.service';
import {CustomerType} from './customer-type';

@Injectable()
export class CustomerTypeService extends BaseService<CustomerType> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag() : string {
        return super.getApiTag() + 'customer-types';
    }
}