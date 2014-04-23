CREATE TABLE [dbo].[tmp_calendar] (
    [date]    DATE NOT NULL,
    [weekday] INT  NOT NULL,
    CONSTRAINT [PK_calendar] PRIMARY KEY CLUSTERED ([date] ASC)
);

