import { BaseModel } from './base.model';
import { User } from './user.model';
import { Organization } from './organization.model';

export class Package extends BaseModel {
    constructor(
        public id: number,
        public caption: string,
        public date: Date,
        public organization: Organization,
        public controller: User,
        public comment: string
    ) {
        super(id, caption);
    }
}