/****** Object:  Table [dbo].[TimeEntryTypes]    Script Date: 11/09/2009 15:48:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TimeEntryTypes](
	[TimeEntryTypeId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IsDefault] [nchar](10) NOT NULL,
	[IsBillableByDefault] [bit] NOT NULL,
 CONSTRAINT [PK_TimeEntryTypes] PRIMARY KEY CLUSTERED 
(
	[TimeEntryTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TimeEntryTypes]  WITH CHECK ADD  CONSTRAINT [FK_CustomersTimeEntryTypes_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerID])
GO

ALTER TABLE [dbo].[TimeEntryTypes] CHECK CONSTRAINT [FK_CustomersTimeEntryTypes_Customers]
GO

  insert into TimeEntryTypes (Name,isdefault,IsBillableByDefault) 
  values ('Regular','1','1')
  
  GO