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
        if (event.formatted != "" && event.epoc != 0) this.context.model.date = new Date(event.date.year, event.date.month, event.date.day);
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
                if (this.context.model.organization != null) this.selected_organization = this.context.model.organization.id;
            });

        //Загружаем дела принадлежащие пакету
        if (this.context.model.id != null)
            this.busy = this._packageFileService.getById(this.context.model.id).then(x => {
                this.context.model.files = x["data"];
            })
    }

    editActionOfPackage(action: Action)
    {
        this.modal
        .open(ActionEditComponent,overlayConfigFactory({ title: 'Редактирование', model: action }, BSModalContext))
        .then(x => {
                x.result.then(output => {
                    console.log(output);
                    if (output != null) {
                        this.busy = this._actionService.put(output)
                            .then(x => {
                                // Object.keys(output).forEach((key) => {
                                //     if (key === 'id') {
                                //         return;
                                //     }
                                //     action[key] = output[key];
                                // });
                            }).catch(x => this.handlerError(x));
                    }
                }, () => null);
            }).catch(this.handlerError);
    }

    protected handlerError(error: any) {
        console.log('CustomerTypeListComponent::error ' + error);
    }

    // edit(model: TModel) {
    //     let clone: TModel = this.cloneModel(model);
    //     this.modal
    //         .open(
    //         this.getEditComponent(),
    //         overlayConfigFactory({ title: 'Редактирование', model: clone }, BSModalContext)
    //         ).then(x => {
    //             x.result.then(output => {
    //                 if (output != null) {
    //                     this.busyMessage = SAVE_MESAGE;
    //                     this.busy = this.service.put(output)
    //                         .then(x => {
    //                             Object.keys(output).forEach((key) => {
    //                                 if (key === 'id') {
    //                                     return;
    //                                 }
    //                                 model[key] = output[key];
    //                             });
    //                         }).catch(x => this.handlerError(x));
    //                 }
    //             }, () => null);
    //         }).catch(this.handlerError);
    // }
}