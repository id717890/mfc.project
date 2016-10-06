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
        //this.items = this.organizationTypeService.getItems();
        //this.items.push('asd');
        this.refreshList();
    }

    private refreshList(): void {
        this.organizationTypes = this.organizationTypeService.getItems();
        //this.organizationTypes.push(new OrganizationType(1, 'item 1'));
        //this.organizationTypes.push(new OrganizationType(2, 'item 2'));
        //this.organizationTypeService.getOrganizationTypes().then(organization_types_list => this.organizationTypes = organization_types_list);
    }
}