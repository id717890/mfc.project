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

export class FileListComponent extends BaseListComponent<File> implements AfterViewInit {
    dateBegin: string;
    dateEnd: string;

    //Fields
    fileStatuses: FileStatus[];
    organizations: Organization[];
    controllers: User[];
    experts: User[];
    services: Service[];

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

    private prepareForm(): void {
        let today = new Date();
        let tomorrow = new Date();
        tomorrow.setDate(today.getDate() + 1);
        this.dateBegin = today.getDate() + '.' + (today.getMonth() + 1) + '.' + today.getFullYear();
        this.dateEnd = tomorrow.getDate() + '.' + (tomorrow.getMonth() + 1) + '.' + tomorrow.getFullYear();
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
        return new File(null, '', null, null, null, null, null, null);
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
            model.organization
        );
    };

    getEditComponent(): any {
        return FileEditComponent;
    }
} 