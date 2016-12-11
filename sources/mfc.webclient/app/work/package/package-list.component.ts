import { Component, AfterViewInit } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { PackageEditComponent } from './package-edit.component';
import { DateService } from './../../infrastructure/assistant/date.service';

import { Organization } from '../../models/organization.model';
import { OrganizationService } from '../../admin/organizations/organization.service';
import { User } from '../../models/user.model';
import { UserService } from '../../admin/users/user.service';
import { Package } from '../../models/package.model';
import { PackageService } from './package.service';

import { DIALOG_CONFIRM, DIALOG_DELETE, SAVE_MESAGE, LOAD_LIST_MESAGE, PAGIN_PAGE_SIZE } from '../../Infrastructure/application-messages';

@Component({
    selector: 'mfc-package-list',
    templateUrl: 'app/work/package/package-list.component.html'
})

export class PackageListComponent extends BaseListComponent<Package> implements AfterViewInit {
    dateBegin: string;
    dateEnd: string;

    //Fields
    organizations: Organization[];
    controllers: User[];

    selectedOrganization: number = -1;
    selectedController: number = -1;

    /* Настройки для datepicker */
    myDatePickerOptions = {
        todayBtnTxt: 'Today',
        dateFormat: 'dd.mm.yyyy',
        firstDayOfWeek: 'mo',
        inline: false
    };

    constructor(public modal: Modal, private packageService: PackageService
        , private organizationService: OrganizationService
        , private userService: UserService
        , private _dateService: DateService
    ) {
        super(modal, packageService);
        this.prepareForm();
    }

    public ngAfterViewInit() {
        this.fillComboboxLists();
    }

    //Обновить фильтр
    Search() {
        this.busy = this.packageService.getWithParameters(this.prepareDataForSearch()).then(x => {
            this.models = x['data'];
            this.totalRows = x['total'];
        })
    }

    //Перелистывание страницы
    getPage(page: number) {
        this.pageIndex = page;
        this.busy =
            this.packageService.getWithParameters(this.prepareDataForSearch()).then(x => {
                this.models = x['data'];
                this.totalRows = x['total'];
            })
    }

    //Собитие изменения даты начала диапазона
    onChangedDateBegin(event: any) {
        if (event.formatted != "") this.dateBegin = event.formatted;
    }

    //Собитие изменения даты окончания диапазона
    onChangedDateEnd(event: any) {
        if (event.formatted != "") this.dateEnd = event.formatted;
    }

    private fillComboboxLists() {
        this.busy = this.organizationService.get().then(x => this.organizations = x["data"]);    //получаем список ОГВ  для combobox
        this.busy = this.userService.get().then(x => {
            let objects = x["data"];
            this.controllers = [];
            for (let item of objects) {
                if ((<User>item).is_controller === true) this.controllers.push(item);
            }
        });    //получаем список Контролеров  для combobox
    }

    //Подготавливаем данные для обновления фильтра
    private prepareDataForSearch(): any[] {
        let param = [];
        param["pageIndex"] = this.pageIndex;
        param["pageSize"] = this.pageSize;
        param["beginDate"] = this.dateBegin;
        param["endDate"] = this.dateEnd;
        param["organization"] = this.selectedOrganization;
        param["controller"] = this.selectedController;
        return param;
    }

    private prepareForm(): void {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        this.dateBegin = this._dateService.ConvertDateToString(firstDay);
        this.dateEnd = this._dateService.ConvertDateToString(lastDay);
    }

    newModel(): Package {
        return new Package(null, '', null, null, null, '', null);
    };

    cloneModel(model: Package): Package {
        return new Package(
            model.id,
            model.caption,
            model.date,
            model.organization,
            model.controller,
            model.comment,
            model.files);
    };

    getEditComponent(): any {
        return PackageEditComponent;
    }
}