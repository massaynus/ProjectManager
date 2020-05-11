CREATE TABLE [dbo].[Role] (
    [RoleID]   INT          IDENTITY (1, 1) NOT NULL,
    [RoleName] VARCHAR (25) COLLATE French_CI_AS NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleID] ASC)
);

