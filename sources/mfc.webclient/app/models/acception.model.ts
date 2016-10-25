import { BaseModel } from '../infrastructure/base.component/base-model';
import { CustomerType } from '../admin/customer-types/customer-type';
import { User } from '../admin/users/user.model';
import { ActionType } from '../admin/actiontypes/actiontype.service';

export class Acception extends BaseModel {
    constructor(
        public id: number,
        public caption: string,
        public date: Date,
        public customer: string,
        public customer_type: CustomerType,
        public action_type: ActionType,
        public expert: User,
        // public service: Service,
        // public service_child: Service,
        public comments: string,
        public is_non_resident: boolean,
        public is_free_visit: boolean
    ) {
        super(id, caption);
    }
}