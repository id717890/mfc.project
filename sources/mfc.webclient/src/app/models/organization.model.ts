import { BaseModel } from './base.model';
import { OrganizationType } from './organization-type.model';

export class Organization extends BaseModel {
    constructor(
        public id: number,
        public caption: string,
        public full_caption: string,
        public organization_type: OrganizationType
    ) {
        super(id, caption);
    }
}