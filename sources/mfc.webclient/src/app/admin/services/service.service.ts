import { Injectable }     from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import {BaseService} from './../../infrastructure/base.component/base.service';
import {Service} from './../../models/service.model';

@Injectable()
export class ServiceService extends BaseService<Service> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag() : string {
        return super.getApiTag() + 'services';
    }
}