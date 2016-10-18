import { Component, Output, Input, EventEmitter } from "@angular/core";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { BaseService } from './base.service';
import {BaseModel} from './base-model';

export class BaseEditContext<TModel extends BaseModel> extends BSModalContext {
    public title: string;
    public model: BaseModel;
}

export class BaseEditComponent<TModel extends BaseModel> implements CloseGuard, ModalComponent<BaseEditContext<TModel>> {
    context: BaseEditContext<TModel>;
    is_changing: boolean;

    public caption: string;

    constructor(public dialog: DialogRef<BaseEditContext<TModel>>) {
        this.context = dialog.context;

        this.is_changing = !dialog.context.model;
        this.caption = this.is_changing ? dialog.context.model.caption : '';
        
        dialog.setCloseGuard(this);
    }

    beforeDismiss(): boolean {
        return false;
    }

    beforeClose(): boolean {
        return this.caption !== '';
    }
}