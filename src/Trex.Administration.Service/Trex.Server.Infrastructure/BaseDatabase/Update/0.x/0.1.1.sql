USE [trex_base]
GO

/****** Object:  StoredProcedure [dbo].[spGetAllPermissions]    Script Date: 03/24/2011 10:20:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetAllPermissions]
AS
select Permissions.Permission from Permissions
GO


           INSERT INTO [Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])

     VALUES
           ('0.1.1.0'
           ,GETDATE()
           ,'djo',
           'new stored procedure spGetAllPermissions')
