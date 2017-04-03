import { BaseModel } from './base.model';
import { User } from './user.model';
import { Organization } from './organization.model';
import { File } from './file.model';

export class Package extends BaseModel {
    constructor(
        public id: number,
        public caption: string,
        public date: Date,
        public organization: Organization,
        public controller: User,
        public comment: string,
        public files: File[]
    ) {
        super(id, caption);
    }
}