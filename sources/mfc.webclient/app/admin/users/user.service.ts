import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import 'rxjs/add/operator/toPromise';

export class User {
    constructor(public id: number, public user_name: string, public description: string, public is_admin: boolean) { }
}

@Injectable()
export class UserService {
    constructor(private http: Http) {
    }

    getUsers(): Promise<User[]> {
        return this.http.get('http://localhost:4664/api/user') //Нужно будет адрес сервера API http://localhost:4664 прописать где-то в конфиге, а пока херачим так ))
            .toPromise()
            .then(res => res.json())
            .catch(this.handlerError)
    }

    private handlerError(error: any) {
        console.log(error);
    }
}