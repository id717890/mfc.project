import { Component, AfterViewInit, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { CompleterService, CompleterData } from 'ng2-completer';

import { Action } from '../../models/action.model';
import { ActionService } from './action.service';
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
import { DateService } from './../../infrastructure/assistant/date.service';

import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';
import { ActionContext } from './action-edit.context';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/work/action/action-edit.component.html',
    providers: [Modal]
})

export class ActionEditComponent extends BaseEditComponent<Action> implements AfterViewInit, OnInit {
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

    constructor(public dialog: DialogRef<ActionContext>
        , private actionService: ActionService
        , private _userService: UserService
        , private _customerTypeService: CustomerTypeService
        , private _actionTypeService: ActionTypeService
        , private _organizationService: OrganizationService
        , private _serviceService: ServiceService
        , private completerService: CompleterService
        , private _dateService: DateService
    ) {
        super(dialog);
    }

    ngOnInit() {
        //Выставляем дату если при создании/редакитровании пакета
        let today: Date = null;
        if (this.context.model.date != null)
            today = new Date(this.context.model.date);
        else today = new Date();
        this.currentDate = this._dateService.ConvertDateToString(today);
        this.context.model.date = today;
    }

    ngAfterViewInit() {
        this.fillForm();
    }

    onChangeDate(event: any) {
        if (event.formatted != "" && event.epoc != 0) {
            this.currentDate = event.formatted;
            this.context.model.date = new Date(event.date.year, event.date.month-1, event.date.day);
        }
        else {
            let today = new Date();
            this.currentDate = this._dateService.ConvertDateToString(today);
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
            this.busy = this._serviceService.getWithParameters(this.prepareDataForServiceChild(service.organization.id, service.id)).then(x => {
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

    private fillForm(): void {
        //получаем список типов приемов для combobox, если действие редактирование заполняем поле
        this.busy =
            this._actionTypeService.get().then(x => {
                this.dialog.context.actionTypes = x['data'];
                var actionTypes = this.dialog.context.actionTypes;
                if (actionTypes != null) {
                    if (this.dialog.context.model.action_type != null) {
                        var id = this.dialog.context.model.action_type.id;
                        for (var item of actionTypes) {
                            if (item.id == id) {
                                this.dialog.context.model.action_type = item;
                                break;
                            }
                        }
                    } else this.context.model.action_type = actionTypes[0];
                }
            });

        //получаем список ОГВ для combobox, если действие редактирование заполняем поле
        this.busy =
            this._organizationService.get().then(x => {
                this.organizations = x["data"];
                if (this.context.model.service != null) this.selected_organization = this.context.model.service.organization.id;
                else this.selected_organization = 0;
            });

        //получаем список пользоввателей для combobox, если действие редактирование заполняем поле
        this.busy =
            this._userService.get().then(x => {
                this.dialog.context.users = x['data'];
                var users = this.dialog.context.users;
                if (users != null) {
                    if (this.dialog.context.model.expert != null) {
                        var id = this.dialog.context.model.expert.id;
                        for (var item of users) {
                            if (item.id == id) {
                                this.dialog.context.model.expert = item;
                                break;
                            }
                        }
                    } else this.context.model.expert = users[0];
                }
            });

        //получаем список Категорий заявителей для combobox, если действие редактирование заполняем поле
        this.busy = this._customerTypeService.get().then(x => {
            this.dialog.context.customerTypes = x['data'];
            var customerTypes = this.dialog.context.customerTypes;
            if (customerTypes != null) {
                if (this.dialog.context.model.customer_type != null) {
                    var id = this.dialog.context.model.customer_type.id;
                    for (var item of customerTypes) {
                        if (item.id == id) {
                            this.dialog.context.model.customer_type = item;
                            break;
                        }
                    }
                } else this.context.model.customer_type = customerTypes[0];
            }
        });

        //Загрузка поля услгуи , если действие редактирование заполняем поле
        let service = this.dialog.context.model.service;
        if (service != null) {
            // this.selected_organization=service.organization; //выставляем выбранный ОГВ            
            this.busy = this._serviceService.getWithParameters(this.prepareData(service.organization.id)).then(x => {
                this.services = x['data'];
                this.autoCompleteServices = this.completerService.local(this.services, 'caption', 'caption');
                this.selected_service = service.caption;
            })
        }

        //Загрузка поля подуслгуи, если действие редактирование заполняем поле
        let service_child = this.dialog.context.model.service_child;
        if (service_child != null) {
            this.busy = this._serviceService.getWithParameters(this.prepareDataForServiceChild(service.organization.id, service.id)).then(x => {
                this.service_childs = x['data'];
                this.autoCompleteServicesChild = this.completerService.local(this.service_childs, 'caption', 'caption');
                this.selected_service_child = service_child.caption;
            })
        } else if (this.dialog.context.model.service != null) this.selected_service_child = "Нет подуслуг";
        else this.selected_service_child = "";
    }
}