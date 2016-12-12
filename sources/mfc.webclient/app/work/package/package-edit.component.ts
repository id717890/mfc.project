import { Component, AfterViewInit, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard, overlayConfigFactory } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { CompleterService, CompleterData } from 'ng2-completer';

import { Package } from '../../models/package.model';
import { PackageService } from './package.service';
import { PackageFileService } from './package-file.service';
import { Organization } from '../../models/organization.model';
import { OrganizationService } from '../../admin/organizations/organization.service';
import { Action } from '../../models/action.model';
import { ActionService } from '../action/action.service';
import { ActionEditComponent } from '../action/action-edit.component';
import { File } from '../../models/file.model';
import { FileService } from '../file/file.service';
import { Messages } from '../../infrastructure/application-messages';

import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';
import { DateService } from './../../infrastructure/assistant/date.service';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/work/package/package-edit.component.html',
    providers: [Modal]
})

export class PackageEditComponent extends BaseEditComponent<Package> implements AfterViewInit, OnInit {
    organizations: Organization[];

    selected_organization: number = -1;

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

    constructor(public modal: Modal
        , public dialog: DialogRef<BaseEditContext<Package>>
        , private _packageService: PackageService
        , private _packageFileService: PackageFileService
        , private _organizationService: OrganizationService
        , private _actionService: ActionService
        , private _fileService: FileService
        , private _dateService: DateService
    ) {
        super(dialog);
    }

    ngOnInit() {
        let today = new Date(this.context.model.date);
        this.currentDate = this._dateService.ConvertDateToString(today);
        this.context.model.date = today;
    }

    ngAfterViewInit() {
        this.fillForm();
    }

    onChangeDate(event: any) {
        if (event.formatted != "" && event.epoc != 0) this.context.model.date = new Date(event.formatted);
        else {
            let today = new Date();
            this.currentDate = today.getDate() + '.' + (today.getMonth() + 1) + '.' + today.getFullYear();
            this.context.model.date = today;
        }
    }

    private fillForm(): void {
        //получаем список ОГВ для combobox, если действие редактирование заполняем поле
        this.busy =
            this._organizationService.get().then(x => {
                this.organizations = x["data"];
                if (this.organizations != null)
                    if (this.context.model.organization != null) {
                        var id = this.dialog.context.model.organization.id;
                        for (var item of this.organizations) {
                            if (item.id == id) {
                                this.dialog.context.model.organization = item;
                                break;
                            }
                        }
                    } else this.context.model.organization = this.organizations[0];
            });

        //Загружаем дела принадлежащие пакету
        if (this.context.model.id != null)
            this.busy = this._packageFileService.getById(this.context.model.id).then(x => {
                this.context.model.files = x["data"];
            }).catch(() => this.context.model.files = [])
    }

    editActionOfPackage(action: Action, file: File) {
        this.modal
            .open(ActionEditComponent, overlayConfigFactory({ title: 'Редактирование', model: action }, BSModalContext))
            .then(x => {
                x.result.then(output => {
                    if (output != null) {
                        this.busy = this._actionService.put(output)
                            .then(x => {
                                if (x["status"] == 200) {
                                    Object.keys(output).forEach((key) => {
                                        if (key === 'id') {
                                            return;
                                        }
                                        file.organization = action.service.organization;
                                    });
                                }
                                else this.handlerError(x["statusText"])
                            }).catch(x => this.handlerError(x));
                    }
                }, () => null);
            }).catch(this.handlerError);
    }

    deleteFileOfPackage(file: File) {
        this.modal
            .confirm()
            .title(Messages.ACTION_CONFIRM)
            .body(Messages.DELETE_CONFIRM)
            .open()
            .then(x => {
                x.result.then(result => {
                    if (!result) return;
                    this.busyMessage = Messages.SAVING;
                    this.busy = this._fileService.delete(file)
                        .then(res => {
                            if (res) {
                                this.context.model.files.splice(this.context.model.files.indexOf(file), 1);
                            }
                        });
                }, () => null);
            });
    }

    protected handlerError(error: any) {
        console.log('PackageEditComponent::error ' + error);
        throw new Error("Ошибка приложения");
    }
}