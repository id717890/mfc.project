import { Component, AfterViewInit, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";

import { Messages } from '../../infrastructure/application-messages';
import { DialogRef, ModalComponent, CloseGuard, overlayConfigFactory } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { CompleterService, CompleterData } from 'ng2-completer';

import { File } from '../../models/file.model';
import { FileService } from './file.service';
import { FileControlEditContext, FileControlEditComponent } from './file-control-edit.component';
import { FileHistoryComponent, FileHistoryContext } from './file-history.component';
import { Action } from '../../models/action.model';
import { ActionService } from '../action/action.service';
import { ActionEditComponent } from '../action/action-edit.component';

ActionEditComponent

import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';

import { FileStageConstants } from '../../infrastructure/constants/file-stage.constants';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/work/file/file-edit.component.html',
    providers: [Modal],
    styles: [`
        .btn-action, .btn { width: 100%; margin-bottom: 10px; }
        .btn-action { padding: 4px 8px; }
    `]
})

export class FileEditComponent extends BaseEditComponent<File> implements AfterViewInit, OnInit {
    busy: Promise<any>;
    busyMessage: string = "Загрузка списков...";



    constructor(public modal: Modal
        , public dialog: DialogRef<BaseEditContext<File>>
        , private _fileService: FileService
        , private _actionService: ActionService
    ) {
        super(dialog);
    }

    ngOnInit() {

    }

    ngAfterViewInit() {
    }

    ChangeStatus(status: string): void {
        this.modal
            .open(FileControlEditComponent, overlayConfigFactory({ title: '', model: '', file: this.context.model, status: status }, FileControlEditContext))
            .then(x => {
                x.result.then(output => {
                    this.busy = this._fileService.postStatus(this.context.model.id, status, output)
                        .then(x => {
                            if (x.status == 200) {
                                this.busy = this._fileService.getById(this.context.model.id)
                                    .then(x => {
                                        this.context.model.status = x["data"].status;
                                    })
                                this.dialog.close(this.context.model);
                            }
                            else this.handlerError(x)
                        }).catch(x => this.handlerError(x));
                }, () => null);
            }).catch(this.handlerError);
    }

    editActionOfFile(action: Action) {
        this.modal
            .open(ActionEditComponent, overlayConfigFactory({ title: 'Редактирование', model: action }, BSModalContext))
            .then(x => {
                x.result.then(output => {
                    if (output != null) {
                        this.busy = this._actionService.put(output)
                            .then(x => {
                                if (x["status"] == 200) {
                                    this._fileService.getById(this.context.model.id)
                                        .then(x => {
                                            this.context.model = x["data"];
                                        });
                                    this.dialog.close(this.context.model);
                                }
                                else this.handlerError(x["statusText"])
                            }).catch(x => this.handlerError(x));
                    }
                }, () => null);
            }).catch(this.handlerError);
    }

    historyFile(id: number)
    {
        this.modal
            .open(FileHistoryComponent, overlayConfigFactory({ id: this.context.model.id }, BSModalContext))
            .then(x => {
                x.result.then(()=>null, () => null);
            }).catch(this.handlerError);

    }

    protected handlerError(error: any) {
        console.log('FileEditComponent::error ' + error);
        throw new Error("Ошибка приложения");
    }
}