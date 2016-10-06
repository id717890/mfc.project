import { Component, Output, EventEmitter } from '@angular/core';
import {OnInit} from '@angular/core';

import { OrganizationTypeService } from '../organization-type.service';
import { OrganizationType } from '../organizationType.model';

@Component({
    selector: 'mfc-organizationType-list',
    templateUrl: 'app/admin/organization-types/organization-type-list/organization-type-list.component.html'
})

export class OrganizationTypeListComponent {
    organizationTypes: OrganizationType[] = [];
    items: string[];
    selectedOrganizationType: OrganizationType;
    
    constructor(private organizationTypeService: OrganizationTypeService){
        this.refreshList();
    }

    private refreshList(): void {
        this.organizationTypeService.getOrganizationType().then(orientation_types_list => this.organizationTypes = orientation_types_list);
    }
}