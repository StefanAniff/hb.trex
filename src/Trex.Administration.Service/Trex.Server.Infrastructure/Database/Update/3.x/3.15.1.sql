/*
   19. december 2013 
   User: IVA 
   Database: Trex.dk, RaptorTrex, trex_base
   Application: 
*/

-- NB! INSERT IN TREX.DK, RAPTORTREX AND TREX_BASE
INSERT INTO [dbo].[Permissions]
           ([Permission]
           ,[ClientApplicationID])
     VALUES
           ('EditOthersWorkplanPermission'
           ,3) -- WpfClient
GO