import { Component, AfterViewInit, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { FileService } from './file.service';
// import { OrganizationEditComponent } from './organization-edit.component';
import { DialogService } from '../../infrastructure/dialog/dialog.service';
import { File } from './../../models/file.model';
import { OrganizationType } from './../../models/organization-type.model';
import { MdDialog, MdButton, MdDialogRef } from '@angular/material';

import { FileStatusService } from '../../admin/file-statuses/file-status.service';
import { FileStatus } from '../../models/file-status.model';

import { OrganizationService } from '../../admin/organizations/organization.service';
import { Organization } from '../../models/organization.model';

import { UserService } from '../../admin/users/user.service';
import { User } from '../../models/user.model';

import { ServiceService } from '../../admin/services/service.service';
import { Service } from '../../models/service.model';

import { Observable } from 'rxjs/Rx';
import { DateService } from './../../infrastructure/assistant/date.service';

@Component({
    selector: 'mfc-organization-list',
    templateUrl: 'app/work/file/file-list.component.html'
})

export class FileListComponent extends BaseListComponent<File> implements AfterViewInit, OnInit {
    dateBegin: Date;
    dateEnd: Date;

    //Fields
    fileStatuses: FileStatus[];
    organizations: Organization[];
    controllers: User[];
    experts: User[];
    services: Service[];

    selectedStatus: -1;
    selectedOgv: -1;
    selectedExpert: -1;
    selectedController: -1;
    selectedService: Service;

    serviceCtrl: FormControl;
    filteredServices: Observable<Service[]>;;

    constructor(public dialog: MdDialog, protected organizationService: FileService, protected dialogService: DialogService
        , private _fileStatusService: FileStatusService
        , private _organizationService: OrganizationService
        , private _userService: UserService
        , private _serviceService: ServiceService
        , private _fileService: FileService
        , private _dateService: DateService
    ) {
        super(dialog, organizationService, dialogService);
        this.prepareForm();
    }

    ngOnInit() {
        this.busyMessage="Загрузка списка...";
        
        this.serviceCtrl = new FormControl();
        this.filteredServices = this.serviceCtrl.valueChanges
            .startWith(null)
            .map(x => this.filterServices(x));
    }

    ngAfterViewInit() {
        this.fillComboboxLists();
        this.refresh();
    }



    //Подготавливаем данные для обновления фильтра
    private prepareDataForSearch(): any[] {
        let param = [];
        param["pageIndex"] = this.pageIndex;
        param["pageSize"] = this.pageSize;
        param["beginDate"] = this._dateService.ConvertDateToString(this.dateBegin);
        param["endDate"] = this._dateService.ConvertDateToString(this.dateEnd);
        param["status"] = this.selectedStatus;
        param["organization"] = this.selectedOgv;
        param["service"] = this.selectedService ? this.selectedService.id : -1;
        param["expert"] = this.selectedExpert;
        param["controller"] = this.selectedController;
        return param;
    }

    filterServices(caption: string) {
        return caption ? this.services.filter(s => s.caption.indexOf(caption) != -1)
            : this.services;
    }

    displayService(service: Service): string {
        return service ? service.caption : '';
    }

    //Обновить фильтр
    refresh() {
        
        this.busy = this._fileService.getWithParameters(this.prepareDataForSearch()).then(x => {
            let files: File[] = [];
            files = files.concat(x.data);
            this.models = files;
            this.totalRows = x.count;
        })
    }

    //Перелистывание страницы
    getPage(page: number) {
        this.pageIndex = page;
        // this.checkAll = false;
        this.busy =
            this._fileService.getWithParameters(this.prepareDataForSearch()).then(x => {
                let files: File[] = [];
                files = files.concat(x.data);
                this.models = files;
                this.totalRows = x.count;
            })
    }

    private prepareForm(): void {
        var date = new Date();
        this.dateBegin = new Date(date.getFullYear(), date.getMonth(), 1);
        this.dateEnd = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        this.selectedOgv = -1;
        this.selectedStatus = -1;
        this.selectedExpert = -1;
        this.selectedController = -1;
    }

    private fillComboboxLists() {
        this.busy = this._fileStatusService.get().then(x => this.fileStatuses = [FileStatus.AllFileStatuses].concat(x.data));    //получаем список статусов  для combobox
        this.busy = this._organizationService.get().then(x => this.organizations = [Organization.AllOrganizations].concat(x.data));    //получаем список ОГВ  для combobox
        this.busy = this._userService.get().then(x => {
            let users: User[] = [User.AllUser];
            users = users.concat(x.data);

            this.experts = users.filter(x => x.is_expert === true || x.id == -1);
            this.controllers = users.filter(x => x.is_controller === true || x.id == -1);
        });    //получаем список ОГВ  для combobox
        this.busy = this._serviceService.get().then(x => {
            let serv: Service[] = [Service.AllService];
            serv = serv.concat(x.data);
            this.services = serv;
        })
    }

    newModel(): File {
        return new File(null, '', null, null, null, null, null, null, false);
    };

    cloneModel(model: File): File {
        return new File(
            model.id,
            model.caption,
            model.date,
            model.action,
            model.expert,
            model.controller,
            model.status,
            model.organization,
            model.is_selected
        );
    };

    getEditComponent(): any {
        return null;
    }
}