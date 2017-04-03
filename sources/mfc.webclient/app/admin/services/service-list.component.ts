import { Component } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {OnInit} from '@angular/core';
import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import {BaseListComponent} from './../../infrastructure/base.component/base-list.component';

import {ServiceService} from './service.service';
import {Service} from './../../models/service.model';
import {ServiceEditComponent} from './service-edit.component';

@Component({
    templateUrl: 'app/admin/services/service-list.component.html',
    providers: [Modal]
})

export class ServiceListComponent extends BaseListComponent<Service> implements OnInit {
    services: Service[];
    busy: Promise<any>;
    busyMessage: string;


    constructor(public modal: Modal, private serviceService: ServiceService) {
        super(modal, serviceService);
    }

     newModel(): Service {
         return new Service(null, '', null);
     };

     cloneModel(model : Service): Service {
         return new Service(model.id, model.caption, model.organization);
     };

     getEditComponent() : any{
         return ServiceEditComponent;
     }
}