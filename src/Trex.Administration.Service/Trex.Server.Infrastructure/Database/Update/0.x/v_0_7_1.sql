/****** Object:  Table [dbo].[Version]    Script Date: 08/25/2008 16:24:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Version](
	[Version] [nvarchar](50) COLLATE Danish_Norwegian_CI_AS NOT NULL,
	[Date] [datetime] NOT NULL,
	[Creator] [nvarchar](50) COLLATE Danish_Norwegian_CI_AS NOT NULL,
	[Description] [nvarchar](500) COLLATE Danish_Norwegian_CI_AS NULL
) ON [PRIMARY]

GO
INSERT INTO [Trex.Server].[dbo].[Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])
     VALUES
           (<Version, nvarchar(50),'0.7.1.0'>
           ,<Date, datetime,GetDate()>
           ,<Creator, nvarchar(50),'TGA'>
           ,<Description, nvarchar(500),'Versions tabel'>)