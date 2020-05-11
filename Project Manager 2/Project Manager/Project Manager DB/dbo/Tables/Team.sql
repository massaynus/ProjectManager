CREATE TABLE [dbo].[Team] (
    [TeamID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (35) COLLATE French_CI_AS NOT NULL,
    PRIMARY KEY CLUSTERED ([TeamID] ASC)
);

