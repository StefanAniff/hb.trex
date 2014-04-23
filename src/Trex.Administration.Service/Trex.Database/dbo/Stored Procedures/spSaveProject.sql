
CREATE PROCEDURE [dbo].[spSaveProject]
@ProjectId int,
@Name nvarchar(100),
@cigId int,
@changedate DATETIME
AS
BEGIN
	update Projects
	set ProjectName = @Name,
		CustomerInvoiceGroupID = @cigId,
		ChangeDate = @changedate
	where
		ProjectID = @ProjectId
END
