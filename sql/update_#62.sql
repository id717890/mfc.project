use [mfc-project]
--создаем таблицу для справочника типов заявителей/посетителей
create table CustomerTypes (id bigint, caption varchar(250), is_deleted bit default 0)