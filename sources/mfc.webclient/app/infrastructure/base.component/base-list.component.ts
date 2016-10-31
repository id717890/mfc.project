import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import { BaseService } from './base.service';
import { BaseModel } from './../../models/base.model';
import { BaseEditComponent } from './base-edit.component';
import { DIALOG_CONFIRM, DIALOG_DELETE, SAVE_MESAGE, LOAD_LIST_MESAGE, PAGIN_PAGE_SIZE } from './../application-messages';

@Component({
    selector: 'mfc-customer-type-list',
    templateUrl: 'app/admin/customer-types/customer-type-list.component.html',
    providers: [Modal, BaseEditComponent]
})

export abstract class BaseListComponent<TModel extends BaseModel> implements OnInit {
    models: TModel[];
    busy: Promise<any>;
    busyMessage: string;

    totalRows: number;  // общее кол-во строк сущности для компонента ng2-pagination
    pageSize: number = PAGIN_PAGE_SIZE;  // количество элементов на странице
    pageIndex: number = 1; //текущая страница

    constructor(public modal: Modal, private service: BaseService<TModel>) { }

    abstract newModel(): TModel;
    abstract cloneModel(model: TModel): TModel;
    abstract getEditComponent(): any;

    ngOnInit(): void {
        this.busyMessage = LOAD_LIST_MESAGE;
        this.busy = this.service.get()
            .then(models => {
                this.models = models['data'];       // извлекаем массив данных
                this.totalRows = models['total'];   // извлекаем общее кол-во строк сущности для корректного отображения страниц
            });
    }

    add() {
        let model: TModel = this.newModel();
        this.modal
            .open(
            this.getEditComponent(),
            overlayConfigFactory({ title: 'Новый', model: model }, BSModalContext)
            ).then(x => {
                x.result.then(output => {
                    if (output != null) {
                        this.busyMessage = SAVE_MESAGE;
                        this.busy = this.service.post(output)
                            .then(x => { if (x != null) {
                                this.models.push(x);
                                this.totalRows=+1;
                            } })  //Здесь нужна проверка на null, т.к. если API вернул ответ с ошибкой, то х=undefined
                            .catch(x => this.handlerError(x));
                    }
                }, () => null);
            }).catch(this.handlerError);
    }

    edit(model: TModel) {
        let clone: TModel = this.cloneModel(model);
        this.modal
            .open(
            this.getEditComponent(),
            overlayConfigFactory({ title: 'Редактирование', model: clone }, BSModalContext)
            ).then(x => {
                x.result.then(output => {
                    if (output != null) {
                        this.busyMessage = SAVE_MESAGE;
                        this.busy = this.service.put(output)
                            .then(x => {
                                Object.keys(output).forEach((key) => {
                                    if (key === 'id') {
                                        return;
                                    }
                                    model[key] = output[key];
                                });
                            }).catch(x => this.handlerError(x));
                    }
                }, () => null);
            }).catch(this.handlerError);
    }

    delete(model: TModel) {
        this.modal
            .confirm()
            .title(DIALOG_CONFIRM)
            .body(DIALOG_DELETE)
            .open()
            .then(x => {
                x.result.then(result => {
                    if (!result)
                        return;

                    this.busyMessage = SAVE_MESAGE;
                    this.busy = this.service.delete(model)
                        .then(res => {
                            if (res) {
                                this.models.splice(this.models.indexOf(model), 1);
                            }
                        });
                });
            });
    }

    protected handlerError(error: any) {
        console.log('CustomerTypeListComponent::error ' + error);
    }
}