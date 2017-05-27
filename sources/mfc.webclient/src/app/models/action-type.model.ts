import { BaseModel } from './base.model';

export class ActionType extends BaseModel {
    constructor(public id: number, public caption: string, public need_make_file: boolean) { 
        super(id, caption);
    }
}