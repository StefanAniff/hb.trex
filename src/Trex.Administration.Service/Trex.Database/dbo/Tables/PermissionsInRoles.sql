CREATE TABLE [dbo].[PermissionsInRoles] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [RoleID]       INT NOT NULL,
    [PermissionID] INT NOT NULL,
    CONSTRAINT [PK_PermissionsInRoles] PRIMARY KEY CLUSTERED ([ID] ASC)
);

