import { Component, Output, Input, EventEmitter } from "@angular/core";
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';

import { Observable } from 'rxjs/Observable';

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { BaseService } from './base.service';
import { BaseModel } from './../../models/base.model';

export class BaseEditContext<TModel extends BaseModel> extends BSModalContext {
    public title: string;
    public model: BaseModel;
}

export class BaseEditComponent<TModel extends BaseModel> implements CloseGuard, ModalComponent<BaseEditContext<TModel>> {
    context: BaseEditContext<TModel>;
    isShaking: boolean = false;

    constructor(public dialog: DialogRef<BaseEditContext<TModel>>, public formGroup : FormGroup = null)  {
        this.context = dialog.context;

        if (!dialog.context.model)
            dialog.context.model.reset();
        
        dialog.setCloseGuard(this);
        if (formGroup != null)
        {
            formGroup.valueChanges.subscribe((form: any) => this.mapFormToModel(form));
        }
    }

    mapFormToModel(form: any): void {
    }

    beforeDismiss(): boolean {
        return false;
    }

    beforeClose(): boolean {
        if (this.formGroup != null && !this.formGroup.valid) {
            if (this.isShaking)
                return true; 

            let timer = Observable.timer(1000, 1000);
            this.isShaking = true;

            let subscription = timer.subscribe(t => {
                this.isShaking = false;
                subscription.unsubscribe();
            });

            return true;
        }

        return false;
    }
}