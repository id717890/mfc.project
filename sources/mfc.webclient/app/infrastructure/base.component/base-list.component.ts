import { Component } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {OnInit} from '@angular/core';
import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { DialogRef, overlayConfigFactory } from 'angular2-modal';

import {BaseService} from './base.service';
import {BaseModel} from './base-model';
import {BaseEditComponent} from './base-edit.component';
import {SAVE_MESAGE, LOAD_LIST_MESAGE} from './../application-messages';

@Component({
    selector: 'mfc-customer-type-list',
    templateUrl: 'app/admin/customer-types/customer-type-list.component.html',
    providers: [Modal, BaseEditComponent]
})

export abstract class BaseListComponent<TModel extends BaseModel> implements OnInit {
    models: TModel[];
    busy: Promise<any>;
    busyMessage: string;


    constructor(public modal: Modal, private service: BaseService<TModel>) {
    }

    abstract newModel(): TModel;
    abstract cloneModel(model : TModel): TModel;
    abstract getEditComponent(): any;


    ngOnInit(): void {
        this.busyMessage = LOAD_LIST_MESAGE;
        this.busy = this.service.get()
            .then(models => this.models = models);
    }

    add() { 
        let model : TModel = this.newModel();
        this.modal
            .open(
            this.getEditComponent(),
            overlayConfigFactory({ title: 'Новый', model: model }, BSModalContext)
            ).then(x => {
                x.result.then(output => {
                    if (output != null) {
                        this.busyMessage = SAVE_MESAGE;
                        this.busy = this.service.post(output)
                            .then(x => {
                                this.models.push(x);
                            }).catch(x => this.handlerError(x));
                    }
                }, () => null);
            }).catch(this.handlerError);
    }

    edit(model: TModel) {
        let clone : TModel = this.cloneModel(model);
        this.modal
            .open(
            this.getEditComponent(),
            overlayConfigFactory({ title: 'Редактирование', model: clone}, BSModalContext)
            ).then(x => {
                x.result.then(output => {
                    if (output != null) {
                        this.busyMessage = SAVE_MESAGE;
                        this.busy = this.service.put(output)
                            .then(x => {
                                model.caption = output.caption;
                            }).catch(x => this.handlerError(x));
                    }
                }, () => null);
            }).catch(this.handlerError);
    }

    delete(model: TModel) {
        this.busyMessage = SAVE_MESAGE;
        this.busy = this.service.delete(model)
            .then(res => {
                if (res) {
                    this.models.splice(this.models.indexOf(model), 1);
                }
            });
    }

    protected handlerError(error: any) {
        console.log('CustomerTypeListComponent::error ' + error);
    }
}