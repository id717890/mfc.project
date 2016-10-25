import { BaseModel } from './../infrastructure/base.component/base-model';

export class OrganizationType extends BaseModel {
    constructor(
        public id: number,
        public caption: string
    ) {
        super(id, caption);
    }
}