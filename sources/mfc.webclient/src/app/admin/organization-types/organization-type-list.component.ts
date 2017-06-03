//import { Component, Output, EventEmitter } from '@angular/core';
//import {OnInit} from '@angular/core';
// import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { Component } from '@angular/core';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';

import { OrganizationTypeService } from './organization-type.service';
import { OrganizationTypeEditComponent } from './organization-type-edit.component'
import { OrganizationTypeContext } from './organization-type.context'
import { DialogService } from '../../infrastructure/dialog/dialog.service'

import { OrganizationType } from '../../models/organization-type.model';

import { MdDialog, MdButton, MdDialogRef } from '@angular/material';

@Component({
    selector: 'mfc-organizationType-list',
    templateUrl: 'app/admin/organization-types/organization-type-list.component.html'
})

export class OrganizationTypeListComponent extends BaseListComponent<OrganizationType> {
    private dialogRef: any;

    constructor(public dialog: MdDialog, private organizationTypeService: OrganizationTypeService, private dialog_service: DialogService) {
        super(organizationTypeService);

    }

    create_ogv_type() {
        let context = new OrganizationTypeContext(new OrganizationType(null, ''));

        this.dialogRef = this.dialog.open(OrganizationTypeEditComponent,
            {
                height: 'auto',
                width: '600px',
                data: context
            });

        this.dialogRef.afterClosed().subscribe(result => {
            if (result != null) {
                this.organizationTypeService.post(result)
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

    edit_ogv_type(model: OrganizationType) {
        let context = new OrganizationTypeContext(model);

        this.dialogRef = this.dialog.open(OrganizationTypeEditComponent, {
            height: 'auto',
            width: '600px',
            data: context
        });

        this.dialogRef.afterClosed().subscribe(result => {
            if (result != null) {
                this.organizationTypeService.put(result)
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

    delete_ogv_type(model: OrganizationType) {
        console.log(model);

        this.dialog_service
            .confirm('', 'Удалить запись?')
            .subscribe(result => {
                if (result == true) {
                    this.organizationTypeService.delete(model)
                        .then(res => {
                            this.models.splice(this.models.indexOf(model), 1);
                            this.totalRows -= 1;
                        })
                        .catch(this.handlerError);
                }
            });
    }

    newModel(): OrganizationType {
        return new OrganizationType(null, '');
    };

    cloneModel(model: OrganizationType): OrganizationType {
        return new OrganizationType(model.id, model.caption);
    };

    getEditComponent(): any {
        return OrganizationTypeEditComponent;
    }
}