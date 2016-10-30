import {BaseModel} from './base.model';
export class Service extends BaseModel {
    constructor(public id: number, public caption: string, public organization: number, public organization_caption: string) {
        super(id, caption);
    }
}