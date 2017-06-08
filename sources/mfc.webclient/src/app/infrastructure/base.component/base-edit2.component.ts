import { Component, Output, Input, EventEmitter, Inject } from "@angular/core";
import { NgForm } from "@angular/forms";
import { BaseModel } from './../../models/base.model';

import { OrganizationType } from '../../models/organization-type.model';
import { MaterialModule, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { BaseContext } from './base-context.component';

export class BaseEdit2Component<TModel extends BaseModel> {
    public header_text = "";
    public context:BaseContext<TModel>;

    constructor(
        @Inject(MD_DIALOG_DATA) data: BaseContext<TModel>,
        public dialogRef: MdDialogRef<BaseEdit2Component<TModel>>
    ) {
        this.context=data;
        if (data.model.id == null) this.header_text = "Новый тип ОГВ"; else this.header_text = "Редактирование"
    }

    submit(model: TModel) {
        this.dialogRef.close(model)
    }
}