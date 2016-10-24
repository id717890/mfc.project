import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, RequestOptionsArgs, Response } from '@angular/http';
import {BaseModel} from './base-model';

@Injectable()
export class BaseService<TModel extends BaseModel>  {
    protected apiUrl: string = "http://localhost:4664/api/";
    constructor(protected _http: Http) {
    }

    getApiTag(): string {
        return this.apiUrl;
    }

    get(): Promise<TModel[]> {
        return this._http.get(this.getApiTag())
            .toPromise()
            .then(this.extractData)
            .catch(this.handlerError);
    }

    post(model: TModel): Promise<TModel> {
        return this._http.post(this.getApiTag(), JSON.stringify(model))
            .flatMap((x: Response) => {
                var location = x.headers.get('Location');
                return this._http.get(location);
            })
            .map((x: Response) => x.json())
            .toPromise()
            .then(x => x, y => {
                let error = this.extractData(y);
                alert(error);
                console.warn(error);
            })
            .catch(this.handlerError);
    }

    put(model: TModel) {
        return this._http.put(this.getApiTag() + "/" + model.id, JSON.stringify(model))
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    delete(model: TModel): Promise<Boolean> {
        return this._http.delete(`${this.getApiTag()}/${model.id}`)
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