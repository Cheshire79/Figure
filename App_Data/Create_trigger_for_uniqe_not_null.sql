create trigger dbo.allow_unique_Id_Circle
on Figure
for insert, update
AS
set nocount on

IF EXISTS(select 1
from   inserted as i
inner join dbo.Figure as tu
on i.Id_Circle = tu.Id_Circle
group  by tu.Id_Circle
having count(tu.Id_Circle) > 1)
BEGIN
ROLLBACK
RAISERROR('Uniqueness Criteria for Id_Circle got violated.', 16, 1)
END
GO