CREATE TABLE [dbo].[User] (
    [UserID]    INT           IDENTITY (1, 1) NOT NULL,
    [UserName]  VARCHAR (35)  NULL,
    [Password]  VARCHAR (160) NULL,
    [FirstName] VARCHAR (35)  NULL,
    [LastName]  VARCHAR (35)  NULL,
    [BirthDtae] DATE          NULL,
    [GSM]       VARCHAR (15)  NULL,
    [Email]     VARCHAR (200) NULL,
    [RIB]       VARCHAR (23)  NULL,
    [Role]      INT           NULL,
    [Team]      INT           NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC),
    FOREIGN KEY ([Role]) REFERENCES [dbo].[Role] ([RoleID]),
    FOREIGN KEY ([Team]) REFERENCES [dbo].[Team] ([TeamID]),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);

