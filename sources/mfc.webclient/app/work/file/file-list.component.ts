import { Component, AfterViewInit } from "@angular/core";

import { AppSettings } from '../../infrastructure/application-settings';
import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, ModalComponent, CloseGuard, overlayConfigFactory } from 'angular2-modal';
import { CompleterService, CompleterData } from 'ng2-completer';
import { Messages } from '../../infrastructure/application-messages';

import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { FileEditComponent } from './file-edit.component';
import { PackageEditComponent } from '../package/package-edit.component';
import { FileAcceptComponent } from './file-accept.component';

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
import { Package } from '../../models/package.model';
import { PackageService } from '../package/package.service';
import { DateService } from './../../infrastructure/assistant/date.service';

@Component({
    selector: "mfc-file-list",
    templateUrl: "app/work/file/file-list.component.html"
})


export class FileListComponent extends BaseListComponent<File> implements AfterViewInit {
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
    checkAll: boolean = false;

    //Autocomplete
    private autoCompleteServices: CompleterData;

    /* Настройки для datepicker */
    myDatePickerOptions = AppSettings.DEFAULT_DATE_PICKER_OPTION;

    constructor(public modal: Modal, private fileService: FileService
        , private _fileStatusService: FileStatusService
        , private _organizationService: OrganizationService
        , private _userService: UserService
        , private _serviceService: ServiceService
        , private _packageService: PackageService
        , private _completerService: CompleterService
        , private _dateService: DateService
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

    //Создать копию дела
    CopyFiles()
    {
        alert("Функционал в разработке")
    }

    //Принять дела
    AcceptFiles() {
        //Перед приемом проверяем выбрал ли пользователь хотя бы одно дело
        if (this.models.filter(x => x.is_selected).length == 0) {
            this.modal.alert()
                .size('sm')
                .isBlocking(false)
                .showClose(false)
                .keyboard(27)
                .title('Предупреждение!')
                .body('Не выбрано ни одного дела!')
                .open().then(x => { x.result.then(() => null, () => null) });
        }
        else {
            let selectedFiles = this.models.filter(x => x.is_selected);
            this.busy =
                this.fileService.postAcceptFiles(selectedFiles)
                    .then(x => {
                        if (x.status == 200) {
                            let acceptedList: File[];
                            let rejectedList: File[] = [];
                            let responsList = this.fileService.extractData(x)['data'];
                            selectedFiles.forEach(item => {
                                let find = false;
                                responsList.forEach(x => {
                                    if (x.id == item.id) {
                                        find = true;
                                        item.status = x.status;
                                    }
                                })
                                if (find === false) rejectedList.push(item);
                            })
                            acceptedList = responsList;

                            this.modal
                                .open(FileAcceptComponent,
                                overlayConfigFactory({ accept_list: acceptedList, reject_list: rejectedList }, BSModalContext)
                                ).then(x => {
                                    x.result.then(() => null, () => null);
                                }).catch(this.handlerError);
                        }
                    })
        }
    }

    //Создать пакет
    CreatePackage() {
        //Перед созданием пакета проверяем выбрал ли пользователь хотя бы одно дело
        if (this.models.filter(x => x.is_selected).length == 0) {
            this.modal.alert()
                .size('sm')
                .isBlocking(false)
                .showClose(false)
                .keyboard(27)
                .title('Предупреждение!')
                .body('Не выбрано ни одного дела!')
                .open().then(x => { x.result.then(() => null, () => null) });
        }
        else {
            let model = new Package(null, '', null, null, null, '', this.models.filter(x => x.is_selected));
            this.modal
                .open(PackageEditComponent,
                overlayConfigFactory({ title: 'Новый пакет', model: model, }, BSModalContext)
                ).then(x => {
                    x.result.then(output => {
                        if (output != null) {
                            this.busyMessage = Messages.SAVING;
                            this.busy = this._packageService.post(output).catch(x => this.handlerError(x));
                        }
                    }, () => null);
                }).catch(this.handlerError);
        }
    }

    //Выбираем все объекты
    onSelectAllFiles(event: any) {
        if (event.target.checked)
            this.models.forEach(x => x.is_selected = true);
        else
            this.models.forEach(x => x.is_selected = false);

    }

    //Изменение Услуги
    onSelectService(item: any) {
        if (item != null) this.selectedService = item.originalObject.id;
        else this.selectedService = -1;
    }

    //Собитие изменения даты начала диапазона
    onChangedDateBegin(event: any) {
        if (event.formatted != "") this.dateBegin = event.formatted;
        else {
            let today = new Date();
            this.dateBegin = this._dateService.ConvertDateToString(today);
        }
    }

    //Собитие изменения даты окончания диапазона
    onChangedDateEnd(event: any) {
        if (event.formatted != "") this.dateEnd = event.formatted;
        else {
            let today = new Date();
            this.dateEnd = this._dateService.ConvertDateToString(today);
        }
    }

    //Перелистывание страницы
    getPage(page: number) {
        this.pageIndex = page;
        this.checkAll = false;
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
        this.busy = this._fileStatusService.get().then(x => this.fileStatuses = x["data"]);    //получаем список статусов  для combobox
        this.busy = this._organizationService.get().then(x => this.organizations = x["data"]);    //получаем список ОГВ  для combobox
        this.busy = this._userService.get().then(x => {
            let objects = x["data"];
            this.experts = [];
            this.controllers = [];
            for (let item of objects) {
                if ((<User>item).is_expert === true) this.experts.push(item);
                if ((<User>item).is_controller === true) this.controllers.push(item);
            }
        });    //получаем список ОГВ  для combobox
        this.busy = this._serviceService.get().then(x => {
            this.services = x['data'];
            this.autoCompleteServices = this._completerService.local(this.services, 'caption', 'caption');
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
        return FileEditComponent;
    }
} 