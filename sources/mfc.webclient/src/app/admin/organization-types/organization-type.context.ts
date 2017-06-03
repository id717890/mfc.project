import { OrganizationType } from '../../models/organization-type.model';

export class OrganizationTypeContext {
    organization_type: OrganizationType;

    constructor(public object: OrganizationType) {
        this.organization_type = object;
    }
}