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

    addType() {
        var type: CustomerType = new CustomerType();
        type.id = 0;
        type.caption = "Новый";

        this.customerTypeService.addCustomerType(type)
            .then(newType => {
                this.customerTypes.push(newType);
            });
    }

    deleteType(type: CustomerType) {
        this.customerTypeService.deleteCustomerType(type)
            .then(res => {
                if (res) {
                    this.customerTypes.splice(this.customerTypes.indexOf(type), 1);
                }
            });
    }

    private fillCustomerTypes() {
        this.customerTypeService.getCustomerTypes()
            .then(types => this.customerTypes = types);
    }
}