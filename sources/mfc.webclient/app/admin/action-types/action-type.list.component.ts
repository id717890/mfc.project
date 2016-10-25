import { Component, EventEmitter } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import { ActionType } from './action-type.model'
import { ActionTypeService } from './action-type.service'

import { ActionTypeEditComponent } from './action-type.edit.component'

@Component({
    selector: 'mfc-action-type-list',
    templateUrl: 'app/admin/action-types/action-type.list.component.html',
    providers: [Modal]
})

export class ActionTypeListComponent {
    private actionTypes: ActionType[];
    
    constructor(public modal: Modal, private actionTypeService: ActionTypeService) {
        actionTypeService.get()
            .then(x => {
                this.actionTypes = x;
            })
            .catch(this.handlerError);
    }

    dialogNew() {
        this.modal
            .open(
                ActionTypeEditComponent,
                overlayConfigFactory({ title: 'Новый вид деятельности', actionType: new ActionType(null, '', false) }, BSModalContext)
            ).then((x) => {
                return x.result.then((output) => {
                    this.actionTypeService.create(output).then(x => {
                        this.actionTypes.push(x);
                    }).catch(x => this.handlerError(x));
                }, () => null);
            });
    }

    dialogEdit(id: number) {
        let actionType: ActionType = this.actionTypes.find(x => x.id == id);
        this.modal
            .open(
                ActionTypeEditComponent,
                overlayConfigFactory({ title: ('Изменить вид деятельности'), actionType: new ActionType(actionType.id, actionType.caption, actionType.need_make_file) }, BSModalContext)
            ).then((x) => {
                return x.result.then((output) => {
                    actionType.caption = output.caption;
                    actionType.need_make_file = output.need_make_file;
                    this.actionTypeService.update(actionType);
                }, () => null);
            });
    }

    onNewClick(event: Event) {
        this.dialogNew();
    }

    onEditClick(event: Event, actionType: ActionType) {
        this.dialogEdit(actionType.id);
    }

    onDeleteClick(event: Event, actionType: ActionType) {
        this.modal.confirm()
            .showClose(true)
            .title('Предупреждение')
            .body('<h4>Вы действительно желаете удалить запись?</h4>')
            .open()
            .then(x => {
                x.result.then(x => {
                    if (x) {
                        let founded = this.actionTypes.findIndex(x => x.id == actionType.id);
                        this.actionTypeService.delete(this.actionTypes[founded]);
                        delete this.actionTypes.splice(founded, 1);
                    }
                }, () => null)
            });
    }

    private handlerError(error: any) {
        console.log('ActionTypeListComponent::error ' + error);
    }
}