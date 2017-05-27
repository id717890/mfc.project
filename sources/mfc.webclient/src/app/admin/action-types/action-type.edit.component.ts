import { Component, Output, Input, EventEmitter } from "@angular/core";
import { FormBuilder, Validators } from '@angular/forms';

// import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
// import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { ActionType } from '../../models/action-type.model'
import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';

// export class ActionTypeEditContext extends BSModalContext {
//     public title: string;
//     public actionType: ActionType;
// }

@Component({
    selector: 'modal-content',
    templateUrl: 'app/admin/action-types/action-type.edit.component.html'
    //,providers: [ Modal ]
})

export class ActionTypeEditComponent extends BaseEditComponent<ActionType> {
    is_changing: boolean;

    public caption: string;
    public need_make_file: boolean;

    constructor() {
        super();
    }

    public checked(actionType: ActionType) {
        actionType.need_make_file = !actionType.need_make_file;
    }
}