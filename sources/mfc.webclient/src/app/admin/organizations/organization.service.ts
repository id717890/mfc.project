import { Injectable }     from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import {BaseService} from './../../infrastructure/base.component/base.service';
import {Organization} from './../../models/organization.model';

@Injectable()
export class OrganizationService extends BaseService<Organization> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag() : string {
        return super.getApiTag() + 'organizations';
    }
}