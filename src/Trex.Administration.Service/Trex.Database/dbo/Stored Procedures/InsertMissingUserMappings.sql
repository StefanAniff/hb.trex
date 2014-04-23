-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertMissingUserMappings]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO [trex.dk].[dbo].[BugtrackerIntegrationUser]
           ([TrexUserID]
           ,[GeminiUserID])

select * from(
SELECT  [UserID] as [TrexUserID]
     
      ,(SELECT [userid]
      
      
  FROM [GeminiV3].[dbo].[gemini_users] where [username]= u.email) as [GeminiUserID]
   
  FROM [trex.dk].[dbo].[Users] u  where userid not in (SELECT [TrexUserID]
    
  FROM [trex.dk].[dbo].[BugtrackerIntegrationUser]) and userid > 27 ) as s1 where [GeminiUserID] is not null


  INSERT INTO [trex.dk].[dbo].[BugtrackerIntegrationUser]
           ([TrexUserID]
           ,[GeminiUserID])

select * from(
SELECT  [UserID] as [TrexUserID]
     
      ,(SELECT [userid]
      
      
  FROM [GeminiV3].[dbo].[gemini_users] where [username]= u.username) as [GeminiUserID]
   
  FROM [trex.dk].[dbo].[Users] u  where userid not in (SELECT [TrexUserID]
    
  FROM [trex.dk].[dbo].[BugtrackerIntegrationUser]) and userid > 27 ) as s1 where [GeminiUserID] is not null
  
END
