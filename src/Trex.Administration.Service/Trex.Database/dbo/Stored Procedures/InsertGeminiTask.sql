CREATE PROCEDURE [dbo].[InsertGeminiTask]
	 @geminiProjectID numeric(10,0) ,
@geminiIssueID numeric(10,0),
 @summary nvarchar(255),
 @projectCode nvarchar(10),
 @geminiUserID numeric(10,0),
 @estimateHours numeric(10,0)
 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	exec InsertMissingUserMappings
	declare @TrexProjectID int =0
	declare @trextaskID int = 0
 select  @TrexProjectID=[TrexProjectID]
  FROM [dbo].[BugtrackerIntegrationProject] where [GeminiProjectID] = @geminiProjectID
  
  if @TrexProjectID <> 0
  begin
  if not exists  ( select     [TrexTaskID]
      
  FROM [dbo].[BugtrackerIntegrationTask] where [GeminiTaskID]= @geminiIssueID)
 begin
 declare @trexUserID int = 2
	SELECT      @trexUserID = [TrexUserID]
   FROM [dbo].[BugtrackerIntegrationUser] where GeminiUserID = @geminiUserID
 
 
 INSERT INTO [dbo].[Tasks]
           ([ParentID]
           ,[Guid]
           ,[ProjectID]
           ,[CreatedBy]
           ,[ModifyDate]
           ,[CreateDate]
           ,[TaskName]
           ,[Description]
           ,[TimeEstimated]
           ,[TimeLeft]
           ,[Closed]
           ,[WorstCaseEstimate]
           ,[BestCaseEstimate]
           ,[TagID]
           ,[RealisticEstimate]
           ,[Inactive]
           ,[ChangeDate]
           ,[ChangedBy])
     VALUES
           (null
           ,NEWID()
           ,@TrexProjectID
           ,@trexUserID
           ,GETDATE()
           ,GETDATE()
           ,@projectCode +'-'+ cast(@geminiIssueID as nvarchar)+ ' '+@summary
           ,'Created from gemini'
           ,@estimateHours
           ,0
           ,0
           ,0
           ,0
           ,null
           ,0
           ,0
           ,null
           ,null)
          select @trextaskID = cast(@@IDENTITY  as int)
           
           INSERT INTO [dbo].[BugtrackerIntegrationTask]
           ([TrexTaskID]
           ,[GeminiTaskID])
     VALUES
           (@trextaskID
           ,@geminiIssueID)
           
           
 end
  
end
	
END
