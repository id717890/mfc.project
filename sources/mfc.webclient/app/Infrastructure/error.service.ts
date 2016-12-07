import { Http } from "@angular/http";
import { Injectable } from "@angular/core";
import { Response } from "@angular/http";

@Injectable()
export class ErrorLogService {
    constructor(private http: Http) {
    }

    public logError(error: any): void {
        this.sendToConsole(error);
    }

    private sendToConsole(error: any): void {

        if (console && console.group && console.error) {
            console.group("Error Log Service");
            console.error(error);
            console.error(error.message);
            console.error(error.stack);
            console.groupEnd();
        }
    }
}