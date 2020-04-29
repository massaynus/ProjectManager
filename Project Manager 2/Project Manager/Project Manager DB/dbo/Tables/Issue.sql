CREATE TABLE [dbo].[Issue] (
    [IssueID]     INT          IDENTITY (1, 1) NOT NULL,
    [Task]        INT          NULL,
    [Issuer]      INT          NULL,
    [Title]       VARCHAR (30) NULL,
    [Description] TEXT         NULL,
    [isSolved]    BIT          DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([IssueID] ASC),
    FOREIGN KEY ([Issuer]) REFERENCES [dbo].[User] ([UserID]),
    FOREIGN KEY ([Task]) REFERENCES [dbo].[Task] ([TaskID])
);

