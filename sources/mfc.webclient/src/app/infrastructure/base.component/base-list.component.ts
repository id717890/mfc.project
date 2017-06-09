import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BaseService } from './base.service';
import { BaseModel } from './../../models/base.model';
import { BaseEditComponent } from './base-edit.component';
import { BaseContext } from './base-context.component';
import { Messages } from './../application-messages';
import { AppSettings } from './../application-settings';
import { MdDialog, MdButton, MdDialogRef } from '@angular/material';
import { DialogService } from '../../infrastructure/dialog/dialog.service';

@Component({
    selector: 'mfc-base-list'
})

export abstract class BaseListComponent<TModel extends BaseModel> implements OnInit {
    models: TModel[];
    busy: Promise<any>;
    busyMessage: string;
    private dialogRef: any;

    totalRows: number;  // общее кол-во строк сущности для компонента ng2-pagination
    pageSize: number = AppSettings.DEFAULT_PAGE_SIZE;  // количество элементов на странице
    pageIndex: number = 1; //текущая страница

    constructor(public dialog: MdDialog, protected service: BaseService<TModel>, protected dialogService: DialogService) { }

    abstract newModel(): TModel;
    abstract cloneModel(model: TModel): TModel;
    abstract getEditComponent(): any;

    @Output() tested = new EventEmitter();

    ngOnInit(): void {
        this.busyMessage = Messages.LOADING_LIST;

        this.busy = this.service.get()
            .then(models => {
                this.models = models.data
                this.totalRows = models.count
            });
    }

    add(height: string = AppSettings.DEFAULT_DIALOG_HEIGHT, width: string = AppSettings.DEFAULT_DIALOG_WIDTH) {
        let model: TModel = this.newModel();
        let context = new BaseContext(model);
        this.dialogRef = this.dialog.open(this.getEditComponent(),
            {
                height: height,
                width: width,
                data: context
            });

        this.dialogRef.afterClosed().subscribe((result: any) => {
            if (result != null) {
                this.busy = this.service.post(result)
                    .then(x => {
                        if (x != null) {
                            this.models.push(x);
                            this.totalRows += 1;
                        }
                    })  //Здесь нужна проверка на null, т.к. если API вернул ответ с ошибкой, то х=undefined
                    .catch(x => this.handlerError(x));
            }
        });
    }

    edit(model: TModel, height: string = AppSettings.DEFAULT_DIALOG_HEIGHT, width: string = AppSettings.DEFAULT_DIALOG_WIDTH) {
        let clone: TModel = this.cloneModel(model);
        let context = new BaseContext(model);
        this.dialogRef = this.dialog.open(this.getEditComponent(), {
            height: height,
            width: width,
            data: context
        });
        this.dialogRef.afterClosed().subscribe((result: any) => {
            if (result != null) {
                this.busy = this.service.put(result)
                    .then(x => {
                        Object.keys(x).forEach((key) => {
                            if (key === 'id') {
                                return;
                            }
                            model[key] = x[key];
                        });
                    }).catch(x => this.handlerError(x));
            }
        });
    }

    delete(model: TModel) {
        this.dialogService
            .confirm('', 'Удалить запись?')
            .subscribe(result => {
                if (result == true) {
                    this.busy = this.service.delete(model)
                        .then(res => {
                            this.models.splice(this.models.indexOf(model), 1);
                            this.totalRows -= 1;
                        })
                        .catch(this.handlerError);
                }
            });
    }

    protected handlerError(error: any) {
        console.log('CustomerTypeListComponent::error ' + error);
    }
}