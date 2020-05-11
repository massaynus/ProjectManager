CREATE TABLE [dbo].[Project] (
    [ProjectID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (30)   COLLATE French_CI_AS NOT NULL,
    [Description] TEXT           COLLATE French_CI_AS NOT NULL,
    [Owner]       INT            NOT NULL,
    [Team]        INT            NULL,
    [Budget]      DECIMAL (9, 2) NULL,
    [Priority]    INT            NOT NULL,
    [Complexity]  INT            NOT NULL,
    [StartinDate] DATE           NULL,
    [DueDate]     DATE           NULL,
    PRIMARY KEY CLUSTERED ([ProjectID] ASC),
    FOREIGN KEY ([Owner]) REFERENCES [dbo].[User] ([UserID]),
    FOREIGN KEY ([Team]) REFERENCES [dbo].[Team] ([TeamID])
);

