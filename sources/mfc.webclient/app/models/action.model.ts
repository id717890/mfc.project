import { BaseModel } from './base.model';
import { CustomerType } from './customer-type.model';
import { User } from './user.model';
import { ActionType } from './action-type.model';
import { Service } from './service.model';
import { Organization } from './organization.model';

export class Action extends BaseModel {
    constructor(
        public id: number,
        public caption: string,
        public date: Date,
        public customer: string,
        public customer_type: CustomerType,
        public action_type: ActionType,
        public expert: User,
        public organization: Organization,
        public service: Service,
        public service_child: Service,
        public comments: string,
        public is_non_resident: boolean,
        public is_free_visit: boolean,
        public is_selected:boolean
    ) {
        super(id, caption);
    }
}