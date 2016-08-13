use [mfc-project]
--������� ������� ��� ����������� ����� ����������/�����������
create table CustomerTypes (id bigint, caption varchar(250), is_deleted bit default 0)

-- ��������� ������ �� ���������� ���� ����������
alter table Actions add customer_type_id bigint
alter table Actions add free_visit bit default 0

exec sp_rename 'Actions.tree_visit', 'free_visit', 'COLUMN'