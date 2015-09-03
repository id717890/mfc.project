/*
�������� ������ �� ��
*/

set language us_english;

use Damask

--����� ������
declare @ticket nchar(25)
set @ticket = '20150801-000441'

-- �������� ����� � �������� ���� - ������� ��� �������
declare @ticket_use nchar(25)
set @ticket_use = replace(@ticket, ' ' , '') + '%'

-- ���� ��� ������ ������� ����� ������, �� ��������� �����
begin tran

--�������� �� ��������� ������
delete from Terminal where Ticket like ltrim(rtrim(@ticket_use))

-- �������� �� �������
delete from Process where Ticket like ltrim(rtrim(@ticket_use))

if (@@error <> 0)
begin
  rollback tran
  return
end;

commit tran