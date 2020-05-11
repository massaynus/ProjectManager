CREATE TABLE [dbo].[Address] (
    [AddressID] INT           IDENTITY (1, 1) NOT NULL,
    [UserID]    INT           NOT NULL,
    [street]    VARCHAR (120) COLLATE French_CI_AS NULL,
    [ZipCode]   CHAR (8)      COLLATE French_CI_AS NULL,
    [City]      VARCHAR (20)  COLLATE French_CI_AS NULL,
    [State]     VARCHAR (20)  COLLATE French_CI_AS NULL,
    [Country]   VARCHAR (20)  COLLATE French_CI_AS NULL,
    PRIMARY KEY CLUSTERED ([AddressID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);

