import { Component } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {OnInit} from '@angular/core';

import {CustomerTypeService} from './customer-type.service';
import {CustomerType} from './customer-type';

@Component({
    selector: 'mfc-customer-type-list',
    templateUrl: 'app/admin/customer-types/customer-type-list.component.html'
})

export class CustomerTypeListComponent implements OnInit {
    customerTypes: CustomerType[];

    constructor(private customerTypeService: CustomerTypeService) {
    }

    ngOnInit(): void {
        this.fillCustomerTypes();
    }

    private fillCustomerTypes() {
        this.customerTypeService.getCustomerTypes()
            .then(types => this.customerTypes = types);
    }
}