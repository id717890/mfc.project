import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from "rxjs/Observable";

import "rxjs/Rx";
import 'rxjs/add/operator/toPromise';

import { BaseService } from '../../infrastructure/base-service';
import { FileStage } from './file-stage.model';
import { BusyComponent } from '../../Infrastructure/busy-service/busy-service.component';

@Injectable()
export class FileStageService extends BaseService {
    constructor(
        http: Http
        , private _busyService: BusyComponent
    ) {
        super(http);
    }

    get(): Promise<FileStage[]> {
        return this._http.get(this.apiUrl + 'filestage')
            .toPromise()
            .then(this.extractData)
            .catch(this.handlerError)
    }

    getByLocation(url: string): Promise<FileStage> {
        return this._http.get(url)
            .toPromise()
            .then(response => response.json())
            .catch(this.handlerError)
    }

    create(fileStage: FileStage): Promise<FileStage> {
        let body = JSON.stringify(fileStage);

        return this._http.post(this.apiUrl + 'filestage', body)
            .toPromise()
            .then(data => {
                let location = data.headers.get('Location')
                return this.getByLocation(location);
            })
            .catch(this.handlerError)
    }

    update(fileStage: FileStage) {
        this._busyService.Standby("Выполняется сохранение этапов движения дела")
        let body = JSON.stringify(fileStage);
        let url = this.apiUrl + 'filestage/' + fileStage.code;

        return this._http.put(url, body)
            .toPromise()
            .then((response) => {
                this._busyService.Ready();
                if (response.status == 200) return true;
                else {
                    console.log(response);
                    return false;
                }
            }, () => this._busyService.Ready())
            .catch(this.handlerError)
    }

    delete(fileStage: FileStage) {
        let url = this.apiUrl + 'user/' + fileStage.code;

        return this._http.delete(url)
            .toPromise()
            .then(() => true)
            .catch(this.handlerError)
    }

    private extractData(res: Response) {
        return res.json();
    }

    private handlerError(error: any) {
        console.log(error);
    }
}