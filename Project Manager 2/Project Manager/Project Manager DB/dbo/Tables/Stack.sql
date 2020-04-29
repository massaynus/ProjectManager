CREATE TABLE [dbo].[Stack] (
    [StackID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (40) NULL,
    [Tools]   TEXT         NULL,
    PRIMARY KEY CLUSTERED ([StackID] ASC)
);

