import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from "@angular/router";

import { AdminComponent } from './admin.component';
import { CustomerTypeListComponent } from "./customer-types/customer-type-list.component";
import { UserListComponent } from "./users/user-list/user-list.component";
import { FileStatusListComponent } from "./filestatuses/filestatus-list.component";
import { ActionTypeListComponent } from "./actiontypes/actiontype-list.component";
import { OrganizationTypeListComponent } from './organization-types/organization-type-list/organization-type-list.component';

const routes: Routes = [
    {
        path: 'admin',
        component: AdminComponent,
        children: [
            { path: 'users', component: UserListComponent },
            { path: 'customer-types', component: CustomerTypeListComponent },
            { path: 'actiontypes', component: ActionTypeListComponent },
            { path: 'filestatuses', component: FileStatusListComponent },
            { path: 'organization-types', component: OrganizationTypeListComponent },
            { path: 'customer-types', component: CustomerTypeListComponent }
        ]
    }
];

export const adminRouting: ModuleWithProviders = RouterModule.forChild(routes);
