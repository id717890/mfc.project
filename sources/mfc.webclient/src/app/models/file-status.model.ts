import { BaseModel } from './base.model';

export class FileStatus extends BaseModel {
    constructor(public id: number, public caption: string) {
        super(id, caption);
    }
}