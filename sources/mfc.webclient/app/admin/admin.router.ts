import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from "@angular/router";

import { AdminComponent } from './admin.component';
import { CustomerTypeListComponent } from "./customer-types/customer-type-list.component";
import { UserListComponent } from "./users/user-list.component";
import { FileStatusListComponent } from "./file-statuses/file-status.list.component";
import { ActionTypeListComponent } from "./action-types/action-type.list.component";
import { OrganizationTypeListComponent } from './organization-types/organization-type-list/organization-type-list.component';
import { FileStageListComponent } from './file-stages/file-stage-list.component';

const routes: Routes = [
    {
        path: 'admin',
        component: AdminComponent,
        children: [
            { path: 'users', component: UserListComponent },
            { path: 'customer-types', component: CustomerTypeListComponent },
            { path: 'action-types', component: ActionTypeListComponent },
            { path: 'file-statuses', component: FileStatusListComponent },
            { path: 'organization-types', component: OrganizationTypeListComponent },
            { path: 'customer-types', component: CustomerTypeListComponent },
            { path: 'file-stages', component: FileStageListComponent }
        ]
    }
];

export const adminRouting: ModuleWithProviders = RouterModule.forChild(routes);
