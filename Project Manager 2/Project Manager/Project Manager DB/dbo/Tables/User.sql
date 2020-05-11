CREATE TABLE [dbo].[User] (
    [UserID]          INT           IDENTITY (1, 1) NOT NULL,
    [UserName]        VARCHAR (35)  COLLATE French_CI_AS NOT NULL,
    [Password]        VARCHAR (160) COLLATE French_CI_AS NOT NULL,
    [FirstName]       VARCHAR (35)  COLLATE French_CI_AS NOT NULL,
    [LastName]        VARCHAR (35)  COLLATE French_CI_AS NOT NULL,
    [BirthDtae]       DATE          NOT NULL,
    [Sexe]            CHAR (1)      COLLATE French_CI_AS NOT NULL,
    [GSM]             VARCHAR (15)  COLLATE French_CI_AS NOT NULL,
    [Email]           VARCHAR (200) COLLATE French_CI_AS NOT NULL,
    [RIB]             VARCHAR (23)  COLLATE French_CI_AS NOT NULL,
    [Role]            INT           NOT NULL,
    [Team]            INT           NULL,
    [isAccountActive] BIT           DEFAULT ((1)) NOT NULL,
    [Leader]          INT           NULL,
    [Manager]         INT           NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC),
    FOREIGN KEY ([Leader]) REFERENCES [dbo].[User] ([UserID]),
    FOREIGN KEY ([Manager]) REFERENCES [dbo].[User] ([UserID]),
    FOREIGN KEY ([Role]) REFERENCES [dbo].[Role] ([RoleID]),
    FOREIGN KEY ([Team]) REFERENCES [dbo].[Team] ([TeamID]),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);


GO

    CREATE TRIGGER DisableAccount
    ON [User]
    INSTEAD OF DELETE
    AS
    BEGIN
        UPDATE [User] SET isAccountActive = 0 WHERE UserID in (SELECT UserID from Deleted)
    END
