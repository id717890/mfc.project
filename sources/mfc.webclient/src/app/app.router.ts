import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
//import { AdminComponent } from "./admin/admin.component";
import { WorkComponent } from "./work/work.component";
//import { ActionListComponent } from "./work/action/action-list.component";
//import { FileListComponent } from "./work/file/file-list.component";
//import { PackageListComponent } from "./work/package/package-list.component";

const appRoutes: Routes = [
  { path: 'work', component: WorkComponent },
  /*{ path: 'admin', component: AdminComponent },
  { path: 'actions', component: ActionListComponent },
  { path: 'files', component: FileListComponent },
  { path: 'packages', component: PackageListComponent },*/
  { path: '', component: WorkComponent }
];

export const appRoutingProviders: any[] = [

];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);