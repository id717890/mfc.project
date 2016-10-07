import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import { User } from './user.model';
import { Password } from './password.model';
import {BaseService} from './../../infrastructure/base-service';

@Injectable()
export class UserService extends BaseService {
    constructor(http: Http) {
        super(http);
    }

    getUsers(): Promise<User[]> {
        return this._http.get(this.apiUrl + 'user')
            .toPromise()
            .then(this.extractData)
            .catch(this.handlerError)
    }

    getUser(url: string): Promise<User> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this._http.get(url, options)
            .toPromise()
            .then(response => response.json())
            .catch(this.handlerError)
    }

    createUser(user: User): Promise<User> {
        let body = JSON.stringify(user);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this._http.post(this.apiUrl + 'user', body, options)
            .toPromise()
            .then(data => {
                let location = data.headers.get('Location')
                return this.getUser(location);
            })
            .catch(this.handlerError)
    }

    changePassword(user: User, password: Password) {
        let body = JSON.stringify(password.password);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        let url = this.apiUrl + 'password/' + user.id;

        this._http.put(url, body, options)
            .toPromise()
            .then((response) => {
                if (response.status == 200) return true;
                else {
                    console.log(response);
                    return false;
                }
            })
            .catch(() => this.handlerError)
    }

    updateUser(user: User) {
        let body = JSON.stringify(user);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        let url = this.apiUrl + 'user/' + user.id;

        return this._http.put(url, body, options)
            .toPromise()
            .then(x => true)
            .catch(this.handlerError)
    }

    deleteUser(user: User) {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        let url = this.apiUrl + 'user/' + user.id;

        return this._http.delete(url, options)
            .toPromise()
            .then(() => true)
            .catch(this.handlerError)
    }

    private extractData(res: Response) {
        return res.json();
    }

    private handlerError(error: any) {
        if (error.status === 409) {
            alert(error.json());
        }
        else {
            console.log(error);
        }
    }
}