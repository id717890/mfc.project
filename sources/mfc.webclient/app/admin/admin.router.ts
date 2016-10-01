import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from "@angular/router";

import { AdminComponent } from './admin.component';
import { UsersComponent } from "./users/users.component";

const routes: Routes = [
    {
        path: 'admin',
        component: AdminComponent,
        children: [
            { path: 'users', component: UsersComponent }
        ]
    }
];

export const adminRouting: ModuleWithProviders = RouterModule.forChild(routes);
