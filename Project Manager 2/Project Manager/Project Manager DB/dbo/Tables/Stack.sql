CREATE TABLE [dbo].[Stack] (
    [StackID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (40) COLLATE French_CI_AS NOT NULL,
    [Tools]   TEXT         COLLATE French_CI_AS NULL,
    PRIMARY KEY CLUSTERED ([StackID] ASC)
);

