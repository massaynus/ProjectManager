CREATE TABLE [dbo].[Address] (
    [AddressID] INT           IDENTITY (1, 1) NOT NULL,
    [UserID]    INT           NULL,
    [street]    VARCHAR (120) NULL,
    [ZipCode]   CHAR (8)      NULL,
    [City]      VARCHAR (20)  NULL,
    [State]     VARCHAR (20)  NULL,
    [Country]   VARCHAR (20)  NULL,
    PRIMARY KEY CLUSTERED ([AddressID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);

