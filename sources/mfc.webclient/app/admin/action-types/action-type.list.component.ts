import { Component, EventEmitter } from '@angular/core';
import { OnInit } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import { BaseListComponent} from './../../infrastructure/base.component/base-list.component';

import { ActionType } from '../../models/action-type.model'
import { ActionTypeService } from './action-type.service'

import { ActionTypeEditComponent } from './action-type.edit.component'

@Component({
    selector: 'mfc-action-type-list',
    templateUrl: 'app/admin/action-types/action-type.list.component.html',
    providers: [Modal],
    styles: [`
        .glyphicon-ok-sign { color: green; font-size: 1.1em; }
        .glyphicon-minus-sign { color: silver }
    `],
})

export class ActionTypeListComponent extends BaseListComponent<ActionType> implements OnInit {
    busy: Promise<any>;
    busyMessage: string;

    constructor(public modal: Modal, private actionTypeService: ActionTypeService) {
        super(modal, actionTypeService);
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