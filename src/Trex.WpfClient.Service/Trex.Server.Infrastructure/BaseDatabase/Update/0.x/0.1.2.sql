USE [trex_base]
GO

/****** Object:  StoredProcedure [dbo].[spGetConnection]    Script Date: 08/23/2011 08:50:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[spGetConnection]
@CustomerID varchar(100)
AS
SELECT connectionstring FROM customers WHERE CustomerId = @CustomerID



GO


  INSERT INTO [Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])

     VALUES
           ('0.1.2.0'
           ,GETDATE()
           ,'djo',
           'new stored procedure spGetConnection')
GO