import { BaseEditContext } from './../../infrastructure/base.component/base-edit.component';

import { Action } from '../../models/action.model';
import { CustomerType } from '../../models/customer-type.model';
import { User } from '../../models/user.model';
import { ActionType } from '../../models/action-type.model';

export class ActionContext extends BaseEditContext<Action> {
    customerTypes: CustomerType[];
    users: User[];
    actionTypes: ActionType[];
}