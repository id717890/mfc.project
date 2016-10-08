import { Injectable }     from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import {BaseService} from './../../infrastructure/base-service';
import {CustomerType} from './customer-type';

@Injectable()
export class CustomerTypeService extends BaseService {
    private _apiControllerUrl: string = this.apiUrl + 'customer-types';

    constructor(http: Http) {
        super(http);
    }
    getCustomerTypes(): Promise<CustomerType[]> {
        return this._http.get(this._apiControllerUrl)
            .toPromise()
            .then(this.extractData)
            .catch(this.handlerError);
    }

    addCustomerType(customerType: CustomerType): Promise<CustomerType> {
        return this._http.post(this._apiControllerUrl, JSON.stringify(customerType))
            .flatMap((x: Response) => {
                var location = x.headers.get('Location');
                return this._http.get(location);
            })
            .map((x: Response) => x.json())
            .toPromise()
            .then(x => x)
            .catch(this.handlerError);
    }

    updateCustomerType(customerType: CustomerType) {
        return this._http.put(this._apiControllerUrl + "/" + customerType.id, JSON.stringify(customerType))
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    deleteCustomerType(customerType: CustomerType): Promise<Boolean> {
        return this._http.delete(`${this._apiControllerUrl}/${customerType.id}`)
            .toPromise()
            .then(res => true)
            .catch(this.handlerError);
    }

    private extractData(res: Response) {
        return res.json();
    }

    private handlerError(error: any) {
        console.log(error.message);
    }
}