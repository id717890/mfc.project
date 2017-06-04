import {BaseModel} from './base.model';
import { Organization } from './organization.model';

export class Service extends BaseModel {
    constructor(public id: number, public caption: string, public organization: Organization) {
        super(id, caption);
    }
}