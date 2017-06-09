import { BaseModel } from './base.model';

export class FileStatus extends BaseModel {
    constructor(public id: number, public caption: string) {
        super(id, caption);
    }
    public static AllFileStatuses: FileStatus = new FileStatus(-1, "Все");
}