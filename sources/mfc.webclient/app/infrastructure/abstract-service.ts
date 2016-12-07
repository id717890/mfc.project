import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { AppSettings } from './application-settings';

@Injectable()
export class AbstractService {
    protected apiUrl: string = AppSettings.API_ENDPOINT;
    
    constructor(protected _http: Http) {
    }
}