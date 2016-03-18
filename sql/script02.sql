/*
Удаление талона из БД
*/

set language us_english;

use Damask

--Номер талона
declare @ticket nchar(25)
set @ticket = '20150801-000441'

-- приводим талон к удобному виду - удаляем все пробелы
declare @ticket_use nchar(25)
set @ticket_use = replace(@ticket, ' ' , '') + '%'

-- Если при работе скрипта будет ошибка, то выполняем откат
begin tran

--удаление из терминала записи
delete from Terminal where Ticket like ltrim(rtrim(@ticket_use))

-- удаление из приемов
delete from Process where Ticket like ltrim(rtrim(@ticket_use))

if (@@error <> 0)
begin
  rollback tran
  return
end;

commit tran