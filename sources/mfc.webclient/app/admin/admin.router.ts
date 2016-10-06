import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from "@angular/router";

import { AdminComponent } from './admin.component';
import { CustomerTypeListComponent } from "./customer-types/customer-type-list.component";
import { UserListComponent } from "./users/user-list/user-list.component";
import { FileStatusListComponent } from "./filestatuses/filestatus-list.component";

const routes: Routes = [
    {
        path: 'admin',
        component: AdminComponent,
        children: [
            { path: 'users', component: UserListComponent },
            { path: 'customer-types', component: CustomerTypeListComponent },
            { path: 'filestatuses', component: FileStatusListComponent }
        ]
    }
];

export const adminRouting: ModuleWithProviders = RouterModule.forChild(routes);
