import { Component } from '@angular/core';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { ActionType } from '../../models/action-type.model'
import { ActionTypeService } from './action-type.service'
import { ActionTypeEditComponent } from './action-type.edit.component'
import { MdDialog, MdButton, MdDialogRef } from '@angular/material';
import { DialogService } from '../../infrastructure/dialog/dialog.service';

@Component({
    selector: 'mfc-action-type-list',
    templateUrl: 'app/admin/action-types/action-type.list.component.html',
    styles: [`
        .glyphicon-ok-sign { color: green; font-size: 1.1em; }
        .glyphicon-minus-sign { color: silver }
    `],
})

export class ActionTypeListComponent extends BaseListComponent<ActionType> {
    busy: Promise<any>;
    busyMessage: string;

    constructor(public dialog: MdDialog, protected actionTypeService: ActionTypeService, protected dialogService: DialogService) {
        super(dialog, actionTypeService, dialogService);
    }

    newModel(): ActionType {
        return new ActionType(null, '', false);
    };

    cloneModel(model: ActionType): ActionType {
        return new ActionType(model.id, model.caption, model.need_make_file);
    };

    getEditComponent(): any {
        return ActionTypeEditComponent;
    }
}