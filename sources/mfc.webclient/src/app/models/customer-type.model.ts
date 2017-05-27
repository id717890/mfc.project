import { BaseModel } from './base.model';

export class CustomerType extends BaseModel {
    constructor(public id: number, public caption: string) {
        super(id, caption);
    }
}