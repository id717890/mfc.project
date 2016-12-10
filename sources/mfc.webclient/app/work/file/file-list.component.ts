import { Component, AfterViewInit } from "@angular/core";

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { CompleterService, CompleterData } from 'ng2-completer';

import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { FileEditComponent } from './file-edit.component';

import { File } from '../../models/file.model';
import { FileService } from './file.service';
import { FileStatus } from '../../models/file-status.model';
import { FileStatusService } from '../../admin/file-statuses/file-status.service';
import { Organization } from '../../models/organization.model';
import { OrganizationService } from '../../admin/organizations/organization.service';
import { User } from '../../models/user.model';
import { UserService } from '../../admin/users/user.service';
import { Service } from '../../models/service.model';
import { ServiceService } from '../../admin/services/service.service';

@Component({
    selector: "mfc-file-list",
    templateUrl: "app/work/file/file-list.component.html"
})


export class FileListComponent extends BaseListComponent<File>  implements AfterViewInit {
    dateBegin: string;
    dateEnd: string;

    //Fields
    fileStatuses: FileStatus[];
    organizations: Organization[];
    controllers: User[];
    experts: User[];
    services: Service[];

    selectedFileStatus: number = -1;
    selectedOrganization: number = -1;
    selectedExpert: number = -1;
    selectedController: number = -1;
    selectedService: number = -1;
    checkAll:boolean=false;

    //Autocomplete
    private autoCompleteServices: CompleterData;

    /* Настройки для datepicker */
    myDatePickerOptions = {
        todayBtnTxt: 'Today',
        dateFormat: 'dd.mm.yyyy',
        firstDayOfWeek: 'mo',
        inline: false
    };

    constructor(public modal: Modal, private fileService: FileService
        , private fileStatusService: FileStatusService
        , private organizationService: OrganizationService
        , private userService: UserService
        , private serviceService: ServiceService
        , private completerService: CompleterService
    ) {
        super(modal, fileService);
        this.prepareForm();
    }

    public ngAfterViewInit() {
        this.fillComboboxLists();
    }

    //Обновить фильтр
    Search() {
        this.busy = this.fileService.getWithParameters(this.prepareDataForSearch()).then(x => {
            this.models = x['data'];
            this.totalRows = x['total'];
        })
    }

    //Создать пакет
    CreatePackage(){
        this.modal.prompt().open();

        
        console.log(this.models.filter(x => x.is_selected));
    }

    //Выбираем все объекты
    onSelectAllFiles(event:any){
        if (event.target.checked)
            this.models.forEach(x=>x.is_selected=true);
        else 
            this.models.forEach(x=>x.is_selected=false);
        
    }

    //Изменение Услуги
    onSelectService(item: any) {
        if (item != null) this.selectedService = item.originalObject.id;
        else this.selectedService = -1;
    }

    //Собитие изменения даты начала диапазона
    onChangedDateBegin(event: any) {
        if (event.formatted != "") this.dateBegin = event.formatted;
    }

    //Собитие изменения даты окончания диапазона
    onChangedDateEnd(event: any) {
        if (event.formatted != "") this.dateEnd = event.formatted;
    }

    //Перелистывание страницы
    getPage(page: number) {
        this.pageIndex = page;
        this.checkAll=false;
        this.busy =
            this.fileService.getWithParameters(this.prepareDataForSearch()).then(x => {
                this.models = x['data'];
                this.totalRows = x['total'];
            })
    }

    //Подготавливаем данные для обновления фильтра
    private prepareDataForSearch(): any[] {
        let param = [];
        param["pageIndex"] = this.pageIndex;
        param["pageSize"] = this.pageSize;
        param["beginDate"] = this.dateBegin;
        param["endDate"] = this.dateEnd;
        param["status"] = this.selectedFileStatus;
        param["organization"] = this.selectedOrganization;
        param["service"] = this.selectedService;
        param["expert"] = this.selectedExpert;
        param["controller"] = this.selectedController;
        return param;
    }

    private prepareForm(): void {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        this.dateBegin =
            (firstDay.getDate().toString().length == 1 ? "0" + firstDay.getDate() : firstDay.getDate()) + '.'
            + (((firstDay.getMonth() + 1)).toString().length == 1 ? "0" + (firstDay.getMonth() + 1) : (firstDay.getMonth() + 1)) + '.'
            + firstDay.getFullYear();
        this.dateEnd = lastDay.getDate() + '.'
            + (((lastDay.getMonth() + 1)).toString().length == 1 ? "0" + (lastDay.getMonth() + 1) : (lastDay.getMonth() + 1)) + '.'
            + lastDay.getFullYear();
    }

    private fillComboboxLists() {
        this.busy = this.fileStatusService.get().then(x => this.fileStatuses = x["data"]);    //получаем список статусов  для combobox
        this.busy = this.organizationService.get().then(x => this.organizations = x["data"]);    //получаем список ОГВ  для combobox
        this.busy = this.userService.get().then(x => {
            let objects = x["data"];
            this.experts = [];
            this.controllers = [];
            for (let item of objects) {
                if ((<User>item).is_expert === true) this.experts.push(item);
                if ((<User>item).is_controller === true) this.controllers.push(item);
            }
        });    //получаем список ОГВ  для combobox
        this.busy = this.serviceService.get().then(x => {
            this.services = x['data'];
            this.autoCompleteServices = this.completerService.local(this.services, 'caption', 'caption');
        })
    }

    newModel(): File {
        return new File(null, '', null, null, null, null, null, null,false);
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
        return FileEditComponent;
    }
} 