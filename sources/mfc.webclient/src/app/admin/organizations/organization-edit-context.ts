import { Organization } from './../../models/organization.model';
import { BaseEditContext } from './../../infrastructure/base.component/base-edit.component';
import { OrganizationType } from './../../models/organization-type.model';

export class OrganizationEditContext extends BaseEditContext<Organization> {
    organizationTypes : OrganizationType[];
}