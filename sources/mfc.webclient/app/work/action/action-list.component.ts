import { Component } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { ActionEditComponent } from './action-edit.component';

import { Action } from '../../models/action.model';
import { User } from '../../models/user.model';
import { ActionService } from './action.service';
import { UserService } from '../../admin/users/user.service';
import { Messages } from '../../Infrastructure/application-messages';

@Component({
    selector: 'mfc-action-list',
    templateUrl: 'app/work/action/action-list.component.html'
})

export class ActionListComponent extends BaseListComponent<Action> {
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

    constructor(public modal: Modal, private actionService: ActionService, private _userService: UserService) {
        super(modal, actionService);
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

    onChangeExp(user_id: any) {
        console.log(user_id);
    }

    Search() {
        this.busy = this.actionService.getWithParameters(this.prepareData()).then(x => {
            this.models = x['data'];
            this.totalRows = x['total'];
        })
    }

    //Перелистывание страницы
    getPage(page: number) {
        this.pageIndex = page;
        this.busy =
            this.actionService.getWithParameters(this.prepareData()).then(x => {
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

    newModel(): Action {
        return new Action(null, '', null, '', null, null, null, null, null, null, '', false, false);
    };

    cloneModel(model: Action): Action {
        return new Action(
            model.id,
            model.caption,
            model.date,
            model.customer,
            model.customer_type,
            model.action_type,
            model.expert,
            model.service.organization,
            model.service,
            model.service_child,
            model.comments,
            model.is_non_resident,
            model.is_free_visit);
    };

    getEditComponent(): any {
        return ActionEditComponent;
    }

    delete(model: Action) {
        this.modal
            .confirm()
            .title(Messages.ACTION_CONFIRM)
            .body(Messages.DELETE_CONFIRM)
            .open()
            .then(x => {
                x.result.then(result => {
                    if (!result)
                        return;

                    this.busyMessage = Messages.SAVING;
                    this.busy = this.actionService.delete(model)
                        .then(res => {
                            if (res) {
                                this.models.splice(this.models.indexOf(model), 1);
                                this.totalRows -= 1;
                            }
                        });
                }, () => null);
            });
    }
}