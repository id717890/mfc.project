import { Injectable }     from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch'

import {BaseService} from './../../infrastructure/base-service';
import {CustomerType} from './customer-type';

@Injectable()
export class CustomerTypeService extends BaseService {
    constructor(http: Http) {
        super(http);
    }
    getCustomerTypes(): Promise<CustomerType[]> {
        return this._http.get(this.apiUrl + 'customer-types')
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