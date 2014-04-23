CREATE TABLE [dbo].[Holidays] (
    [HolidayId]   INT          IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME     NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Holidays] PRIMARY KEY CLUSTERED ([HolidayId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Date]
    ON [dbo].[Holidays]([Date] ASC);

