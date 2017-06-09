import { Component, AfterViewInit } from '@angular/core';
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


@Component({
    selector: 'mfc-organization-list',
    templateUrl: 'app/work/file/file-list.component.html'
})

export class FileListComponent extends BaseListComponent<File> implements AfterViewInit {
    dateBegin: Date;
    dateEnd: Date;

    //Fields
    fileStatuses: FileStatus[];
    organizations: Organization[];
    controllers: User[];
    experts: User[];

    selectedStatus: -1;
    selectedOgv: -1;
    selectedExpert: -1;
    selectedController: -1;

    // services: Service[];

    constructor(public dialog: MdDialog, protected organizationService: FileService, protected dialogService: DialogService
        , private _fileStatusService: FileStatusService
        , private _organizationService: OrganizationService
        , private _userService: UserService
    ) {
        super(dialog, organizationService, dialogService);
        this.prepareForm();
    }

    ngAfterViewInit() {
        this.fillComboboxLists();
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
        // this.busy = this._serviceService.get().then(x => {
        //     this.services = x['data'];
        //     this.autoCompleteServices = this._completerService.local(this.services, 'caption', 'caption');
        // })
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