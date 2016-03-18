/*
������������ ������� ������ �����������: ����� ������ ������������ = ����� ������

*/

set language us_english;

use Damask

-- ��� ������ �� ������� ����� ���������
declare @service_id int
set @service_id = 42

--������ �������������
declare @dt_begin datetime, @dt_end datetime
set @dt_begin = cast('2015-07-01 00:00:00' as datetime)
set @dt_end = cast('2016-01-31 23:59:59' as datetime)

-- ���� ��� ������ ������� ����� ������, �� ��������� �����
begin tran

-- ������ ���� ������� �� ��������� ������ �� �������� ������
declare @cr_tickets cursor

set @cr_tickets = cursor FAST_FORWARD for
	select p1.DateTime as dt_call, p1.Ticket, p2.DateTime as dt_start
	from Process p1
		join Process p2 on p2.Ticket = p1.Ticket and p2.Event = 14 /*������� ������ ������������*/
	where p1.ServiceId = @service_id
		and p1.Event = 1 /*������� ������*/
		and cast(p1.DateTime as date) = cast(p2.datetime as date)
		and cast(p1.DateTime as datetime) between @dt_begin and @dt_end;

declare @c_dt_call nchar(19), @c_dt_start nchar(19), @ticket nchar(25)

open @cr_tickets

fetch next from @cr_tickets into @c_dt_call, @ticket, @c_dt_start

-- ������ ������� ��� ��������� ��������� ���������
create table #fault (ticket nchar(25), dt_call datetime, dt_start datetime)

while @@FETCH_STATUS = 0
begin

	update Process set DateTime = @c_dt_call
	where Ticket = @ticket
		and event = 14

		insert into #fault (ticket, dt_call, dt_start) values (@ticket, cast(@c_dt_call as datetime), cast(@c_dt_start as datetime))

	fetch next from @cr_tickets into @c_dt_call, @ticket, @c_dt_start
end

close @cr_tickets
deallocate @cr_tickets

if (@@error <> 0)
begin
  rollback tran
  return
end;

-- ������� ���������� ������
select *
from #fault

drop table #fault

commit tran