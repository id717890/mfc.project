import { BaseModel } from './base.model';
import { FileStatus } from './file-status.model';
import { User } from './user.model';

export class FileStatusHistory extends BaseModel {
    constructor(
        public id: number
        , public caption: string
        , public date: Date
        , public status: FileStatus
        , public user: User
        , public comments: string
    ) {
        super(id, caption);
    }
}