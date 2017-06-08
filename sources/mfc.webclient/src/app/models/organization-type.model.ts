import { BaseModel } from './base.model';

export class OrganizationType extends BaseModel {
    constructor(
        public id: number,
        public caption: string
    ) {
        super(id, caption);
    }

    public static AllOrganizationType: OrganizationType = new OrganizationType(null, "Все");
}