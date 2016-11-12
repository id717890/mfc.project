import { Component, AfterViewInit, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { CompleterService, CompleterData } from 'ng2-completer';

import { Acception } from '../../models/acception.model';
import { AcceptionService } from './acception.service';
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

@Component({
    selector: 'modal-content',
    templateUrl: 'app/work/acception/acception-edit.component.html',
    providers: [Modal]
})

export class AcceptionEditComponent extends BaseEditComponent<Acception> implements AfterViewInit, OnInit {
    experts: User[];
    customerTypes: CustomerType[];
    actionTypes: ActionType[];
    organizations: Organization[];
    services: Service[];
    service_childs: Service[];
    selected_service: string = null;
    selected_service_child: string = null;
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

    constructor(public dialog: DialogRef<BaseEditContext<Acception>>
        , private acceptionService: AcceptionService
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
        let today = new Date();
        this.currentDate = today.getDate() + '.' + (today.getMonth() + 1) + '.' + today.getFullYear();
        this.context.model.date = today;
    }

    ngAfterViewInit() {
        this.fillLists();
    }

    onChangeDate(event: any) {
        if (event.formatted != "" && event.epoc != 0) this.context.model.date = new Date(event.date.year, event.date.month, event.date.day);
        else {
            let today = new Date();
            this.currentDate = today.getDate() + '.' + (today.getMonth() + 1) + '.' + today.getFullYear();
            this.context.model.date = today;
        }
    }

    onChangeOrganization(organization: number) {
        if (organization != 0) {
            this.busy = this._serviceService.getWithParameters(this.prepareData(organization)).then(x => {
                this.services = x['data'];
                this.autoCompleteServices = this.completerService.local(this.services, 'caption', 'caption');
                this.selected_service = "";
                this.selected_service_child = "";
                this.context.model.service_child = null;
            })
        } else {
            this.services = null;
            this.service_childs = null;
            this.selected_service = "";
            this.selected_service_child = "";
            this.context.model.service = null;
            this.context.model.service_child = null;
        }
    }

    onSelectService(item: any) {
        if (item != null) {
            let service: Service = item.originalObject;
            this.context.model.service = service;
            this.busy = this._serviceService.getWithParameters(this.prepareDataForServiceChild(service.organization, service.id)).then(x => {
                let data = x['data'];
                this.service_childs = data;
                this.autoCompleteServicesChild = this.completerService.local(this.service_childs, 'caption', 'caption');
                this.selected_service_child = "";
                this.context.model.service_child = null;
                if (data.length == 0) this.selected_service_child = "Нет подуслуг"
            })
        }
    }

    onSelectServiceChild(item: any) {
        item != null ? this.context.model.service_child = item.originalObject : this.context.model.service_child = null;
    }

    private prepareData(organizationId: number): any[] {
        let param = [];
        param["organization"] = organizationId;
        return param;
    }

    private prepareDataForServiceChild(organizationId: number, parent: number): any[] {
        let param = [];
        param["organization"] = organizationId;
        param["parent"] = parent;
        return param;
    }

    private fillLists(): void {
        //получаем список ОГВ для combobox
        this.busy =
            this._organizationService.get().then(x => {
                this.organizations = x["data"];
            });

        //получаем список пользоввателей для combobox
        this.busy =
            this._userService.get().then(x => {
                this.experts = x["data"];
                if (this.experts.length != 0) this.context.model.expert = this.experts[0];
            });

        //получаем список Категорий заявителей для combobox
        this.busy = this._customerTypeService.get().then(x => {
            this.customerTypes = x["data"];
            if (this.customerTypes.length != 0) this.context.model.customer_type = this.customerTypes[0];
        });

        //получаем список Типов услуг для combobox
        this.busy = this._actionTypeService.get().then(x => {
            this.actionTypes = x["data"];
            if (this.actionTypes.length != 0) this.context.model.action_type = this.actionTypes[0];
        });
    }
}