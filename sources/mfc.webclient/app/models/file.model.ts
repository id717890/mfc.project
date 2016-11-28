import { BaseModel } from './base.model';
import { Action } from './action.model';
import { User } from './user.model';
import { FileStatus } from './file-status.model';
import { Organization } from './organization.model';

export class File extends BaseModel {
    constructor(
        public id: number,
        public caption: string,
        public date: Date,
        public action: Action,
        public expert: User,
        public controller: User,
        public status: FileStatus,
        public organization: Organization
    ) {
        super(id, caption);
    }
}