import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from "@angular/router";

import { AdminComponent } from './admin.component';
import { UserListComponent } from "./users/user-list.component";

const routes: Routes = [
    {
        path: 'admin',
        component: AdminComponent,
        children: [
            { path: 'users', component: UserListComponent }
        ]
    }
];

export const adminRouting: ModuleWithProviders = RouterModule.forChild(routes);
