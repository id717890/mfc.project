import { BaseEditContext } from './../../infrastructure/base.component/base-edit.component';

import { Acception } from '../../models/acception.model';
import { CustomerType } from '../../models/customer-type.model';
import { User } from '../../models/user.model';
import { ActionType } from '../../models/action-type.model';

export class AcceptionContext extends BaseEditContext<Acception> {
    customerTypes: CustomerType[];
    users: User[];
    actionTypes: ActionType[];
}