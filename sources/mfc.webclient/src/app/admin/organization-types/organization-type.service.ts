import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { OrganizationType } from '../../models/organization-type.model';
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