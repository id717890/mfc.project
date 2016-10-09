import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, RequestOptionsArgs, Response } from '@angular/http';

@Injectable()
export class BaseService  {
    protected apiUrl: string = "http://localhost:4664/api/";
    constructor(protected _http: Http) {
    }
}