import { Component, AfterViewInit, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { CompleterService, CompleterData } from 'ng2-completer';

import { File } from '../../models/file.model';
import { FileService } from './file.service';
import { User } from '../../models/user.model';
import { UserService } from '../../admin/users/user.service';
import { CustomerType } from '../../models/customer-type.model';
import { CustomerTypeService } from '../../admin/customer-types/customer-type.service';
import { ActionType } from '../../models/action-type.model';
import { ActionTypeService } from '../../admin/action-types/action-type.service';
import { Organization } from '../../models/organization.model';
import { OrganizationService } from '../../admin/organizations/organization.service';
import { Service } from '../../models/service.model';
import { ServiceService } from '../../admin/services/service.service';

import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';
import { FileContext } from './file-edit.context';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/work/action/action-edit.component.html',
    providers: [Modal]
})

export class FileEditComponent extends BaseEditComponent<File> implements AfterViewInit, OnInit {
    organizations: Organization[];
    services: Service[];
    service_childs: Service[];

    selected_service: string = null;
    selected_service_child: string = null;
    selected_organization: number;

    private autoCompleteServices: CompleterData;
    private autoCompleteServicesChild: CompleterData;

    currentDate: string;
    busy: Promise<any>;
    busyMessage: string = "Загрузка списков...";

    /* Настройки для datepicker */
    myDatePickerOptions = {
        todayBtnTxt: 'Today',
        dateFormat: 'dd.mm.yyyy',
        firstDayOfWeek: 'mo',
        inline: false,
    };

    constructor(public dialog: DialogRef<FileContext>
        , private actionService: FileService
        , private _userService: UserService
        , private _customerTypeService: CustomerTypeService
        , private _actionTypeService: ActionTypeService
        , private _organizationService: OrganizationService
        , private _serviceService: ServiceService
        , private completerService: CompleterService
    ) {
        super(dialog);
    }

    ngOnInit() {
       
    }

    ngAfterViewInit() {
    }
}