import { Directive, Component, ViewChild, ComponentFactoryResolver, Type, ViewContainerRef } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { OnInit } from '@angular/core';

//import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
//import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import {DialogDirective} from './../../dialog/dialog.directive';

import { CustomerTypeService } from './customer-type.service';
import { CustomerType } from '../../models/customer-type.model';
import { CustomerTypeEditComponent } from './customer-type-edit.component';

import {TestComponent} from './test.component';

@Directive({selector: '[mfc-dialog]'})
class ChildDirective {
}

@Component({
    selector: 'mfc-customer-type-list',
    templateUrl: 'app/admin/customer-types/customer-type-list.component.html'/*,
    providers: [Modal]*/
    
})

export class CustomerTypeListComponent extends BaseListComponent<CustomerType> implements OnInit {
    @ViewChild(TestComponent) dialogHost: TestComponent;
    customerTypes: CustomerType[];

    constructor(/*public modal: Modal, */private customerTypeService: CustomerTypeService, private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef) {
        super(/*modal, */customerTypeService);
    }

    newModel(): CustomerType {
        return new CustomerType(null, '');
    };

    cloneModel(model: CustomerType): CustomerType {
        return new CustomerType(model.id, model.caption);
    };

    getEditComponent(): any {
        return CustomerTypeEditComponent;
    }

    //showDialog = false;

    onShowDialog() {
        let componentFactory  = this.componentFactoryResolver.resolveComponentFactory(this.getEditComponent());
        let viewContainerRef = this.dialogHost.viewContainerRef;
        viewContainerRef.clear();
        viewContainerRef.createComponent(componentFactory);
        
        /*this.showDialog = !this.showDialog;
        console.log(this.showDialog);*/
    }
}