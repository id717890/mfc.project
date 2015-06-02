use [mfc-project]

alter table Users add is_expert bit
alter table Users add is_controller bit

update Users set is_expert = 1 where is_admin = 0 or is_admin is null;
update Users set is_controller = 0

create table Files (Id bigint, dt date, caption varchar(250), action_id bigint, user_id bigint, controller_id bigint, status_id bigint, orv_id bigint, is_deleted bit default 0)

create table FileStatus (file_id bigint, dt datetime, status_id bigint, user_id bigint, comments varchar(250))


create table Statuses (id bigint, caption varchar(250), is_deleted bit default 0)

alter table ActionTypes add make_file bit default 0

update ActionTypes set make_file = 1 where UPPER(REPLACE(caption, '�', '�')) like '�����%';


create table FileStages (code varchar(100), caption varchar(250), status_id bigint, order_id int);


insert into FileStages values ('new', '����� ����', null, 1);
insert into FileStages values ('control', '���������� ��� ��������', null, 2);
insert into FileStages values ('fix', '���������� ��� ���������� ���������', null, 3);
insert into FileStages values ('send', '���������� � ���', null, 4);
insert into FileStages values ('check', '���������', null, 5);

-- ���������
alter table Services add parent_id bigint;

alter table Actions add service_child_id bigint;

-- ���� 
alter table SErvices add code varchar(50)

alter table Actions alter column dt datetime;
alter table Files alter column dt datetime;
alter table FileStatus alter column dt datetime;

alter table Actions add is_nonresident bit default 0;