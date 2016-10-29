import { Component, Output, Input, EventEmitter } from "@angular/core";
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
    templateUrl: 'app/work/acception/acception.edit.component.html',
    providers: [Modal]
})

export class AcceptionEditComponent extends BaseEditComponent<Acception> {
    experts: User[];
    customerTypes: CustomerType[];
    actionTypes: ActionType[];
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
    ) {
        super(dialog);
        this.fillLists();
        //Заполняем форму
        // this.formGroup.patchValue({ caption: dialog.context.model.caption });
    }

    private fillLists(): void {
        this._userService.get().then(x => this.experts = x["data"]);    //получаем список пользоввателей для combobox
        this._customerTypeService.get().then(x => this.customerTypes = x["data"]);    //получаем список Категорий заявителей для combobox
        this._actionTypeService.get().then(x => this.actionTypes = x);    //получаем список Типов услуг для combobox
    }

    // mapFormToModel(form: any): void {
    //     this.context.model.caption = form.caption;
    // }
}