import { Component } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';

import { Package } from '../../models/package.model';
import { PackageService } from './package.service';

import { DIALOG_CONFIRM, DIALOG_DELETE, SAVE_MESAGE, LOAD_LIST_MESAGE, PAGIN_PAGE_SIZE } from '../../Infrastructure/application-messages';

@Component({
    selector: 'mfc-package-list',
    templateUrl: 'app/work/package/package-list.component.html'
})

export class PackageListComponent extends BaseListComponent<Package> {
    constructor(public modal: Modal, private packageService: PackageService) {
        super(modal, packageService);
    }

    newModel(): Package {
        return new Package(null, '', null, null, null,'');
    };

    cloneModel(model: Package ): Package {
        return new Package(
            model.id,
            model.caption,
            model.date,
            model.organization,
            model.user,
            model.comments);
    };

    getEditComponent(): any {
        return null;
    }
}