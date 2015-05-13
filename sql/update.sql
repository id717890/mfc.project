use [mfc-project]

alter table Users add is_expert bit
alter table Users add is_controller bit

update Users set is_expert = 1 where is_admin = 0 or is_admin is null;
update Users set is_controller = 0

create table Files (Id bigint, dt date, caption varchar(250), action_id bigint, user_id bigint, controller_id bigint, status_id bigint, orv_id bigint, is_deleted bit default 0)

create table FileStatus (id bigint, dt date, status_id bigint, user_id bigint, comments varchar(250))


create table Statuses (id bigint, caption varchar(250), is_deleted bit default 0)

alter table ActionTypes add make_file bit default 0

update ActionTypes set make_file = 1 where UPPER(REPLACE(caption, '�', '�')) like '�����%';