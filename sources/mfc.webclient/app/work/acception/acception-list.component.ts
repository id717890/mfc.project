import { Component } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { AcceptionEditComponent } from './acception.edit.component';

import { Acception } from '../../models/acception.model';
import { User } from '../../models/user.model';
import { AcceptionService } from './acception.service';
import { UserService } from '../../admin/users/user.service';

@Component({
    selector: 'mft-acception-list',
    templateUrl: 'app/work/acception/acception-list.component.html'
})

export class AcceptionListComponent extends BaseListComponent<Acception> {
    experts: User[];
    selectedExpert: number = -1;
    dateBegin: string;
    dateEnd: string;

    /* Настройки для datepicker */
    myDatePickerOptions = {
        todayBtnTxt: 'Today',
        dateFormat: 'dd.mm.yyyy',
        firstDayOfWeek: 'mo',
        inline: false,
        // sunHighlight: true,
        // height: '34px',
        width: '200px',
        // selectionTxtFontSize: '16px'
    };

    constructor(public modal: Modal, private acceptionService: AcceptionService, private _userService: UserService) {
        super(modal, acceptionService);
        this.fillLists();
        this.prepareForm();
    }

    private fillLists(): void {
        this.busy = this._userService.get().then(x => this.experts = x["data"]);    //получаем список пользоввателей для combobox
    }

    private prepareForm(): void {
        let today = new Date();
        let tomorrow = new Date();
        tomorrow.setDate(today.getDate() + 1);
        this.dateBegin = today.getDate() + '.' + (today.getMonth() + 1) + '.' + today.getFullYear();
        this.dateEnd = tomorrow.getDate() + '.' + (tomorrow.getMonth() + 1) + '.' + tomorrow.getFullYear();
    }

    onDateBeginChanged(event: any) {
        this.dateBegin = event.formatted;
    }
    onDateEndChanged(event: any) {
        this.dateEnd = event.formatted;
    }

    onExpertChange(user_id: any) {
        this.selectedExpert = user_id;
    }

    onChangeExp(user_id: any){
        console.log(user_id);
    }

    Search() {
        this.busy = this.acceptionService.getWithParameters(this.prepareData()).then(x => {
            this.models = x['data'];
            this.totalRows = x['total'];
        })
    }

    //Перелистывание страницы
    getPage(page: number) {
        this.pageIndex = page;
        this.busy =
            this.acceptionService.getWithParameters(this.prepareData()).then(x => {
                this.models = x['data'];
                this.totalRows = x['total'];
            })
    }

    private prepareData(): any[] {
        let param = [];
        param["dateEnd"] = this.dateEnd;
        param["dateBegin"] = this.dateBegin;
        param["pageIndex"] = this.pageIndex;
        param["pageSize"] = this.pageSize;
        param["userId"] = this.selectedExpert;
        return param;
    }

    newModel(): Acception {
        return new Acception(null, '', null, '', null, null, null, '', false, false);
    };

    cloneModel(model: Acception): Acception {
        return new Acception(
            model.id,
            model.caption,
            model.date,
            model.customer,
            model.customer_type,
            model.action_type,
            model.expert,
            model.comments,
            model.is_non_resident,
            model.is_free_visit);
    };

    getEditComponent(): any {
        return AcceptionEditComponent;
    }
}