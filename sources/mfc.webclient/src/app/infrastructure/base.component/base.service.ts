import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, RequestOptionsArgs, Response, URLSearchParams } from '@angular/http';
import { BaseModel } from './../../models/base.model';

import { AppSettings } from './../application-settings';
import { AbstractService } from './../abstract-service';

@Injectable()
export class BaseService<TModel extends BaseModel> extends AbstractService {
    constructor(protected _http: Http) {
        super(_http);
    }

    getApiTag(): string {
        return this.apiUrl;
    }

    get(): Promise<TModel[]> {
        return this._http.get(this.getApiTag())
            .toPromise()
            .then(x => this.extractData(x))
            .catch(this.handlerError);
    }

    //Для отправки GET запроса с параметрами
    getWithParameters(parameters: any[]): Promise<TModel[]> {
        let params: URLSearchParams = new URLSearchParams();
        Object.keys(parameters).forEach((key) => {
            params.set(key, parameters[key].toString());
        });
        return this._http.get(this.getApiTag(), { search: params })
            .toPromise()
            .then(x => this.extractData(x))
            .catch(this.handlerError);
    }

    post(model: TModel): Promise<TModel> {
        // return null;
        //todo: implements
        return this._http.post(this.getApiTag(), JSON.stringify(model))
            .flatMap((x: Response) => {
                var location = x.headers.get('Location');
                return this._http.get(location);
            })
            .map((x: Response) => x.json())
            .toPromise()
            .then(x => x, y => {
                let error = this.extractData(y);
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

    delete(model: TModel):Promise<boolean> {
        return this._http.delete(`${this.getApiTag()}/${model.id}`)
            .toPromise()
            .then(res => true)
            .catch(this.handlerError);
    }

    extractData(res: Response) {
        /*
        В headers ответа от сервера добавлен заголовок "Total-rows", в который пишется кол-во строк запрошенной сущности.
        Это количество нужно для компонента ng2-pagination, чтобы корректно рассчитывать кол-во страниц.
        После извлечения данных возвращается массив 
        {
            total: count,
            data: Object[] 
        }        
         */
        let output: any[] = [];
        let data = res.json();
        let total: number = +res.headers.get('Total-rows');

        output['total'] = total != null ? total : 0;
        output['data'] = data;

        return output;
    }

    public handlerError(error: any) {
        if (error != null && error.message != null)
            console.log(error.message);
        else if (error != null && error.message == null)
            console.log(error);
        else console.log("Произошла не понятная ошибка");
    }
}