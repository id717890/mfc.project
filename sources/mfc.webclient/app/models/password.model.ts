import { User } from './user.model';

export class Password {
    password: string;
    confirm: string;

    constructor(password: string, confirm: string) {
        this.password=password;
        this.confirm=confirm;
    }
}