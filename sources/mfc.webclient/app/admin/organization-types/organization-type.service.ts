import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch'
import { OrganizationType } from './organizationType.model';
import { BaseService } from './../../infrastructure/base-service';

@Injectable()
export class OrganizationTypeService extends BaseService {
    constructor(http: Http) {
        super(http);
    }

    getOrganizationType() : Promise<OrganizationType[]>{
        return this._http.get(this.apiUrl + 'organizationTypeApi')
            .toPromise()
            .then(this.extractData)
            .catch(this.handlerError);
    }

    private extractData(res: Response) {
        return res.json();
    }

    private handlerError(error: any) {
        console.log(error.message);
    }
}