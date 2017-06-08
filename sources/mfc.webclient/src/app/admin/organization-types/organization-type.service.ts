import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { BaseService } from './../../infrastructure/base.component/base.service';
import { OrganizationType } from '../../models/organization-type.model';

@Injectable()
export class OrganizationTypeService extends BaseService<OrganizationType> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'OrganizationType';
    }
}