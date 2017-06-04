import { Component } from '@angular/core';
import { MdDialog, MdButton, MdDialogRef } from '@angular/material';

import { AppSettings } from '../../infrastructure/application-settings';
//import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
//import { ActionEditComponent } from './action-edit.component';

import { Action } from '../../models/action.model';
import { User } from '../../models/user.model';
import { ActionService } from './action.service';
import { UserService } from '../../admin/users/user.service';
import { Messages } from '../../Infrastructure/application-messages';
import { DateService } from './../../infrastructure/assistant/date.service';
import { DialogService } from '../../infrastructure/dialog/dialog.service';

@Component({
    selector: 'mfc-action-list',
    templateUrl: 'app/work/action/action-list.component.html'
})

export class ActionListComponent /*extends BaseListComponent<Action>*/ {
    experts: User[];
    selectedExpert: number = -1;
    dateBegin: Date;
    dateEnd: Date;
    models: Action[];
    busy: Promise<any>;
    busyMessage: string;
    dialog: MdDialog;

    constructor(
        protected _actionService: ActionService
        , protected dialogService: DialogService
        , private _userService: UserService
        , private _dateService: DateService
    ) {
        //super(dialog, _actionService, dialogService);
        this.fillLists();
        this.prepareForm();
    }

    /*copy() {
        if (this.models.filter(x => x.is_selected).length == 0) {
            this.modal.alert()
                .size('sm')
                .isBlocking(false)
                .showClose(false)
                .keyboard(27)
                .title('Предупреждение!')
                .body('Не выбрано ни одного приема!')
                .open().then(x => { x.result.then(() => null, () => null) });
        }
        else {
            let selectedActions = this.models.filter(x => x.is_selected);
            this.busy =
                this._actionService.postCopyAction(selectedActions)
                    .then((x: any) => {
                        if (x.status == 200) {
                            let responsList = this._actionService.extractData(x)['data'];
                            if (responsList != null) {
                                responsList.forEach((x: any) => {
                                    this.models.push(x);
                                })
                            }
                        }
                    })
        }
    }*/

    //Выбираем все приемы на странице
    onSelectAllFiles(event: any) {
        if (event.target.checked)
            this.models.forEach(x => x.is_selected = true);
        else
            this.models.forEach(x => x.is_selected = false);
    }

    private fillLists(): void {
        this.busy = this._userService.get().then(x => {
            let users: User[] = [User.AllUser];
            users = users.concat(x.data);

            this.experts = users;
        });
    }

    private prepareForm(): void {
        this.dateBegin = new Date();
        this.dateEnd = new Date();
        this.selectedExpert = -1;
    }

    
    refresh() {
        this.busy = this._actionService.getWithParameters(this.prepareData()).then(x => {
            this.models = x.data;
        })
    }

    //Перелистывание страницы
    getPage(page: number) {
        /*this.pageIndex = page;
        this.busy =
            this._actionService.getWithParameters(this.prepareData()).then(x => {
                this.models = x['data'];
                this.totalRows = x['total'];
            })*/
    }

    private prepareData(): any[] {
        let param: any[] = [];
        param["dateEnd"] = this._dateService.ConvertDateToString(this.dateEnd);
        param["dateBegin"] = this._dateService.ConvertDateToString(this.dateBegin);
        //todo: установка значений для пейджинга
        param["pageIndex"] = 0;
        param["pageSize"] = 100;
        param["userId"] = this.selectedExpert;

        return param;
    }

    newModel(): Action {
        return new Action(null, '', null, '', null, null, null, null, null, null, '', false, false, false);
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
            model.is_free_visit,
            model.is_selected);
    };

    getEditComponent(): any {
        //return ActionEditComponent;
        return null;
    }
}