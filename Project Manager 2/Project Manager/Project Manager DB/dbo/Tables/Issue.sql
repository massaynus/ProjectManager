CREATE TABLE [dbo].[Issue] (
    [IssueID]     INT          IDENTITY (1, 1) NOT NULL,
    [Task]        INT          NOT NULL,
    [Issuer]      INT          NOT NULL,
    [Title]       VARCHAR (30) COLLATE French_CI_AS NOT NULL,
    [Description] TEXT         COLLATE French_CI_AS NOT NULL,
    [isSolved]    BIT          DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([IssueID] ASC),
    FOREIGN KEY ([Issuer]) REFERENCES [dbo].[User] ([UserID]),
    FOREIGN KEY ([Task]) REFERENCES [dbo].[Task] ([TaskID])
);

