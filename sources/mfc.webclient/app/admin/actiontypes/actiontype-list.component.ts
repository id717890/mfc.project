import { Component, EventEmitter } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import { ActionType, ActionTypeService } from './actiontype.service'

import { ActionTypeEditComponent } from './actiontype-edit.component'

@Component({
    selector: 'mfc-actiontype-list',
    templateUrl: 'app/admin/actiontypes/actiontype-list.component.html',
    providers: [Modal]
})

export class ActionTypeListComponent {
    private actiontypes: ActionType[];
    
    constructor(public modal: Modal, private actiontypeService: ActionTypeService) {
        actiontypeService.get()
            .then(x => {
                this.actiontypes = x;
            })
            .catch(this.handlerError);
    }

    dialogNew() {
        this.modal
            .open(
                ActionTypeEditComponent,
                overlayConfigFactory({ title: 'Новый вид деятельности', actiontype: new ActionType(null, '', false) }, BSModalContext)
            ).then((x) => {
                return x.result.then((output) => {
                    this.actiontypeService.create(output).then(x => {
                        this.actiontypes.push(x);
                    }).catch(x => this.handlerError(x));
                }, () => null);
            });
    }

    dialogEdit(id: number) {
        let actiontype: ActionType = this.actiontypes.find(x => x.id == id);
        this.modal
            .open(
                ActionTypeEditComponent,
                overlayConfigFactory({ title: ('Изменить вид деятельности'), actiontype: new ActionType(actiontype.id, actiontype.caption, actiontype.need_make_file) }, BSModalContext)
            ).then((x) => {
                return x.result.then((output) => {
                    actiontype.caption = output.caption;
                    actiontype.need_make_file = output.need_make_file;
                    this.actiontypeService.update(actiontype);
                }, () => null);
            });
    }

    onNewClick(event: Event) {
        this.dialogNew();
    }

    onEditClick(event: Event, actiontype: ActionType) {
        this.dialogEdit(actiontype.id);
    }

    onDeleteClick(event: Event, actiontype: ActionType) {
        this.modal.confirm()
            .showClose(true)
            .title('Предупреждение')
            .body('<h4>Вы действительно желаете удалить запись?</h4>')
            .open()
            .then(x => {
                x.result.then(x => {
                    if (x) {
                        let founded = this.actiontypes.findIndex(x => x.id == actiontype.id);
                        this.actiontypeService.delete(this.actiontypes[founded]);
                        delete this.actiontypes.splice(founded, 1);
                    }
                }, () => null)
            });
    }

    private handlerError(error: any) {
        console.log('ActionTypeListComponent::error ' + error);
    }
}