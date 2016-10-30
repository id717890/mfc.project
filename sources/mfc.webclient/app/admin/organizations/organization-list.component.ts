import { Component } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {OnInit} from '@angular/core';
import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import {BaseListComponent} from './../../infrastructure/base.component/base-list.component';

import {OrganizationService} from './organization.service';
import {Organization} from './../../models/organization.model';
import {OrganizationType} from './../../models/organization-type.model';
import {OrganizationEditComponent} from './organization-edit.component';

@Component({
    templateUrl: 'app/admin/organizations/organization-list.component.html',
    providers: [Modal]
})

export class OrganizationListComponent extends BaseListComponent<Organization> implements OnInit {
    organizations: Organization[];
    busy: Promise<any>;
    busyMessage: string;


    constructor(public modal: Modal, private organizationService: OrganizationService) {
        super(modal, organizationService);
    }

     newModel(): Organization {
         return new Organization(null, '', '', null);
     };

     cloneModel(model : Organization): Organization {
         return new Organization(model.id, model.caption, model.full_caption, model.organization_type);
     };

     getEditComponent() : any {
         return OrganizationEditComponent;
     }
}