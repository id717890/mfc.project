import { BaseEditContext } from './../../infrastructure/base.component/base-edit.component';

import { File } from '../../models/file.model';
import { CustomerType } from '../../models/customer-type.model';
import { User } from '../../models/user.model';
import { ActionType } from '../../models/action-type.model';

export class FileContext extends BaseEditContext<File> {
    customerTypes: CustomerType[];
    users: User[];
    actionTypes: ActionType[];
}