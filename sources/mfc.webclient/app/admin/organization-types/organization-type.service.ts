import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch'
import { OrganizationType } from './organizationType.model';
import { BaseService } from './../../infrastructure/base.component/base.service';

@Injectable()
export class OrganizationTypeService extends BaseService<OrganizationType> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'OrganizationType';
    }
}