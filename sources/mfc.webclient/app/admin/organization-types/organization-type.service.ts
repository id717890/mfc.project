import { Injectable } from '@angular/core';

import { OrganizationType } from './organizationType.model';

@Injectable()
export class OrganizationTypeService  {


    getItems() : OrganizationType[]{

        return [
            new OrganizationType(1, 'item 0'),
            new OrganizationType(2, 'item 1'),
            new OrganizationType(3, 'item 2'),
            new OrganizationType(4, 'item 4'),
        ]; 
    }

/*
    getItems() : string[] {
        
        return ['item 0', 'item 1'];
    }
*/
}