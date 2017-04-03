-- пользовательские роли
create table Roles (id bigint, name nvarchar(250), description nvarchar(512));

insert into Roles (id, name) values (1, 'Администратор');
insert into Roles (id, name) values (2, 'Эксперт');
insert into Roles (id, name) values (3, 'Контролер');

-- связь пользовательской роли и системной роли
create table RolesAsn (role_id bigint, sys_role_code nvarchar(250));

insert into RolesAsn (role_id, sys_role_code) values (1, 'admin');
insert into RolesAsn (role_id, sys_role_code) values (2, 'expert');
insert into RolesAsn (role_id, sys_role_code) values (3, 'controller');

-- функциональные привелегии доступа роли к модулю и операциям в модуле
create table RoleModules (role_id bigint, module nvarchar(250));

insert into RoleModules (role_id, module) values (1, 'admin');
insert into RoleModules (role_id, module) values (1, 'file');
insert into RoleModules (role_id, module) values (1, 'action');
insert into RoleModules (role_id, module) values (1, 'report');
insert into RoleModules (role_id, module) values (1, 'customer-type');
insert into RoleModules (role_id, module) values (1, 'package');

create table RoleModuleOperations(role_id bigint, module nvarchar(250), operation nvarchar(250));

insert into RoleModuleOperations(role_id, module, operation) values (1, 'customer-type', 'add');
insert into RoleModuleOperations(role_id, module, operation) values (1, 'customer-type', 'delete');
insert into RoleModuleOperations(role_id, module, operation) values (1, 'customer-type', 'update');

-- привязка пользователя к роли
create table UserRoleAsn(user_id bigint, role_id bigint);

insert into UserRoleAsn values(1, 1);