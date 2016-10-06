import { Component, EventEmitter } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import { FileStatus, FileStatusService } from './filestatus.service'

import { FileStatusEditComponent, FileStatusEditContext } from './filestatus-edit.component'

@Component({
    selector: 'mfc-filestatus-list',
    templateUrl: 'app/admin/filestatuses/filestatus-list.component.html',
    providers: [Modal]
})

export class FileStatusListComponent {
    private filestatuses: FileStatus[];
    
    constructor(public modal: Modal, private fileStatusService: FileStatusService) {
        fileStatusService.get()
            .then(x => {
                this.filestatuses = x;
            })
            .catch(this.handlerError);
    }

    dialogNew() {
        this.modal
            .open(
                FileStatusEditComponent,
                overlayConfigFactory({ title: ('Новый статус'), filestatus: new FileStatus(null, '') }, BSModalContext)
            ).then((x) => {
                return x.result.then((output) => {
                    this.fileStatusService.create(output).then(x => {
                        this.filestatuses.push(x);
                    }).catch(x => this.handlerError(x));
                });
            }, () => null);
    }

    dialogEdit(id: number) {
        console.log('dialog(' + id + ')');
        let filestatus: FileStatus = this.filestatuses.find(x => x.id == id);
        this.modal
            .open(
                FileStatusEditComponent,
                overlayConfigFactory({ title: ('Изменить статус'), filestatus: new FileStatus(filestatus.id, filestatus.caption) }, BSModalContext)
            ).then((x) => {
                return x.result.then((output) => {
                    filestatus.caption = output.caption;
                    this.fileStatusService.update(filestatus);
                });
            }, () => null);
    }

    onNewClick(event: Event) {
        this.dialogNew();
    }

    onEditClick(event: Event, filestatus: FileStatus) {
        this.dialogEdit(filestatus.id);
    }

    onDeleteClick(event: Event, filestatus: FileStatus) {
        this.modal.confirm()
            .showClose(true)
            .title('Предупреждение')
            .body('<h4>Вы действительно желаете удалить запись?</h4>')
            .open()
            .then(x => {
                x.result.then(x => {
                    if (x) {
                        let founded = this.filestatuses.findIndex(x => x.id == filestatus.id);
                        this.fileStatusService.delete(this.filestatuses[founded]);
                        delete this.filestatuses.splice(founded, 1);
                    }
                })
            });
    }

    private handlerError(error: any) {
        console.log('FileStatusListComponent::error ' + error);
    }
}