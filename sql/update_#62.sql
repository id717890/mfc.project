use [mfc-project]
--������� ������� ��� ����������� ����� ����������/�����������
create table CustomerTypes (id bigint, caption varchar(250), is_deleted bit default 0)

-- ��������� ������ �� ���������� ���� ����������
alter table Actions add customer_type_id bigint