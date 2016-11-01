import { Component, Output, Input, EventEmitter, AfterViewInit, OnInit } from "@angular/core";
import { FormBuilder, Validators } from '@angular/forms';

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

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
    styles: [`
        .input-line {
            margin-bottom: 25px;
            width: 100%;
        }
        .input-buttons {
            margin-top: 10px;
        }
    `],
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
    ser: string = "";

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

    constructor(public dialog: DialogRef<BaseEditContext<Acception>>, formBuilder: FormBuilder
        , private acceptionService: AcceptionService
        , private _userService: UserService
        , private _customerTypeService: CustomerTypeService
        , private _actionTypeService: ActionTypeService
        , private _organizationService: OrganizationService
        , private _serviceService: ServiceService
    ) {
        super(dialog, formBuilder.group(
            {
                'customer': [null, Validators.required],
                'comments': [null],
                'is_non_resident': [false],
                'is_free_visit': [false]
            }));
        this.formGroup.patchValue({
            customer: dialog.context.model.customer
        });
    }

    mapFormToModel(form: any): void {
        this.context.model.customer = form.customer;
        this.context.model.comments = form.comments;
        this.context.model.is_non_resident = form.is_non_resident;
        this.context.model.is_free_visit = form.is_free_visit;
    }

    ngOnInit() {
        let today = new Date();
        this.currentDate = today.getDate() + '.' + (today.getMonth() + 1) + '.' + today.getFullYear();
        this.context.model.date = today;
    }

    ngAfterViewInit() {
        this.fillLists();
    }

    onChangeService(service: number) {
        this.context.model.service = this.services.find(x => x.id == service);
    }

    onChangeServiceChild(service_child: number) {
        this.context.model.service_child = this.services.find(x => x.id == service_child);
    }

    onChangeOrganization(organization: number) {
        if (organization != 0) {
            this.busy = this._serviceService.getWithParameters(this.prepareData(organization)).then(x => {
                this.services = x['data'];
                this.service_childs = x['data'];
            })
        } else {
            this.services = null;
            this.service_childs = null;
        }
    }

    onChangeDate(event: any) {
        this.context.model.date = new Date(event.date.year, event.date.month, event.date.day);
    }

    onChangeExpert(userId: any) {
        this.context.model.expert = this.experts.find(x => x.id == userId);
    }

    onChangeActionType(actionType: any) {
        this.context.model.action_type = this.actionTypes.find(x => x.id == actionType);
    }

    onChangeCustomerType(customerType: any) {
        this.context.model.customer_type = this.customerTypes.find(x => x.id == customerType);
    }

    private prepareData(organizationId: number): any[] {
        let param = [];
        param["organization"] = organizationId;
        return param;
    }

    private fillLists(): void {
        // //получаем список услуг для combobox
        // this.busy =
        //     this._serviceService.get().then(x => {
        //         this.services = x["data"];
        //         this.service_childs = x["data"];
        //         if (this.services.length != 0) this.context.model.service = this.services[0];
        //         if (this.services.length != 0) this.context.model.service_child= this.services[0];
        //     });

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