import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from "./admin/admin.component";
import { WorkComponent } from "./work/work.component";
import { AcceptionComponent } from "./work/acception/acception-list.component";

const appRoutes: Routes = [
  { path: 'work', component: WorkComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'acceptions', component: AcceptionComponent },
  { path: '', component: WorkComponent }
];

export const appRoutingProviders: any[] = [

];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);