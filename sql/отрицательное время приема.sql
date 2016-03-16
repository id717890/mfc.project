/*
Поиск талонов со временем вызова меньшим чем время выдачи талона
Время создания талона - Terminal.datetime
Время выдачи талона - Terminal.timecall
Время вызова - Process.datetime
*/

set language us_english;

use Damask


--Период корректировки
declare @dt_begin datetime, @dt_end datetime
set @dt_begin = cast('2001-01-01 00:00:00' as datetime)
set @dt_end = cast('2016-01-31 23:59:59' as datetime)

-- максимальное время между выдачаей талона и вызовом
declare @n_max int
set @n_max = 900 -- 15:00

-- Если при работе скрипта будет ошибка, то выполняем откат
begin tran

-- Курсор всех билетов по указанной услуге за отчетный период
declare @cr_tickets cursor

set @cr_tickets = cursor FAST_FORWARD for
	select datetime, timecall, ticket
	from Terminal
	where cast(datetime as datetime) between @dt_begin and @dt_end

declare @datetime nchar(19), @timecall nchar(19), @ticket nchar(25)

open @cr_tickets

fetch next from @cr_tickets into @datetime, @timecall, @ticket

-- время вызова
declare @dt_start datetime

-- время создания и выдачи билета
declare @dt_create datetime, @dt_call datetime

-- содаем таблицы для просмотра сделанных изменений
create table #fault (ticket nchar(25), dt_create datetime, dt_call datetime, dt_start datetime, new_dt datetime, new_dt_char nchar(19))

while @@FETCH_STATUS = 0
begin
	set @dt_create = cast(@datetime as datetime)
	set @dt_call = cast(@timecall as datetime)

    -- получаем дату вызова на обслуживание
	select @dt_start = cast(datetime as datetime)
	from Process
	where Ticket = @ticket and  event = 1

	if @dt_start < @dt_call
	begin
		insert into #fault (ticket, dt_create, dt_call, dt_start) values (@ticket, @dt_create, @dt_call, @dt_start)
	end

	/*-- Расчитываем новое время записи. Время начала приема - n_max (допустимое время задержки в секундах)
	declare @new_time as datetime
	set @new_time = dateadd(ss, -@n_max, @dt_start)

	-- если реальноев время выдачи меньше чем новое (т.е. событие реальной выдачи талона произошло раньше, чем допустимое время)
	-- это означает, что было нарушение по допустимому времени 
	if  @dt_call < @new_time
	begin
		-- устанавливаем время выдачи талона, равным новому времени (т.е. предельно допустимому)
		update Terminal set TimeCall = convert(nchar(19), @new_time, 120)
		where Ticket = @ticket

		-- добавляем данные по изменениям в лог-таблицу
		insert into #fault (ticket, dt_create, dt_call, dt_start, new_dt, new_dt_char) values (@ticket, @dt_create, @dt_call, @dt_start, @new_time, convert(nchar(19), @new_time, 120))
	end*/
	
	fetch next from @cr_tickets into @datetime, @timecall, @ticket
end

close @cr_tickets
deallocate @cr_tickets

if (@@error <> 0)
begin
  rollback tran
  return
end;

-- выводим измененные записи
select *
from #fault
order by (dt_call - dt_create) desc

drop table #fault

commit tran