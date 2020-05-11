use master
if not Exists(select name from master.dbo.sysdatabases where name ='ProjectManagerDB')
BEGIN
        CREATE DATABASE ProjectManagerDB
        ON  PRIMARY (
            NAME = N'ProjectManagerDB', 
            FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\ProjectManagerDB.mdf' , 
            SIZE = 147456KB ,
            FILEGROWTH = 30720KB 
        )
        LOG ON ( 
            NAME = N'ProjectManagerDB_log', 
            FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\ProjectManagerDB_log.ldf' , 
            SIZE = 20480KB , 
            FILEGROWTH = 20480KB 
        )
END
go

use ProjectManagerDB
go

BEGIN Tran
    BEGIN

        BEGIN TRY
            create table Team (
                TeamID int primary key IDENTITY,
                Name VARCHAR(35) NOT NULL
            )

            create table Stack (
                StackID int PRIMARY key IDENTITY,
                Name VARCHAR(40) NOT NULL,
                Tools text
            )

            create table TeamStack (
                Num int PRIMARY KEY IDENTITY,
                Team int FOREIGN key REFERENCES Team  NOT NULL,
                Stack int FOREIGN key REFERENCES Stack  NOT NULL
            )

            create table [Role](
                RoleID int PRIMARY key IDENTITY,
                RoleName VARCHAR(25)  NOT NULL
            )

            create table [User] (
                UserID int PRIMARY KEY IDENTITY,
                UserName VARCHAR(35) UNIQUE  NOT NULL,
                Password VARCHAR(160)  NOT NULL,
                FirstName VARCHAR(35)  NOT NULL,
                LastName VARCHAR(35)  NOT NULL,
                BirthDtae DATE  NOT NULL,
                Sexe CHAR  NOT NULL,
                GSM VARCHAR(15)  NOT NULL,
                Email VARCHAR(200)  NOT NULL,
                RIB VARCHAR(23)  NOT NULL,
                [Role] INT FOREIGN KEY REFERENCES [Role]  NOT NULL,
                Team INT FOREIGN key REFERENCES Team,
                isAccountActive BIT DEFAULT(1) NOT NULL,
				Leader int FOREIGN KEY References [User],
				Manager int FOREIGN KEY References [User]
            )

            CREATE TABLE [Address] (
                AddressID int PRIMARY key IDENTITY,
                UserID int FOREIGN key REFERENCES [User]  NOT NULL,
                street VARCHAR(120) ,
                ZipCode CHAR(8),
                City VARCHAR(20),
                State VARCHAR(20),
                Country VARCHAR(20) 
            )

            create table Project (
                ProjectID int primary key IDENTITY,
                Name VARCHAR(30)  NOT NULL,
                Description text  NOT NULL,
                Owner int FOREIGN key REFERENCES [User]  NOT NULL,
                Team int FOREIGN key REFERENCES Team,
                Budget DECIMAL(9,2),
                Priority int  NOT NULL,
                Complexity int  NOT NULL,
                StartinDate date,
                DueDate date
            )

            CREATE TABLE Paiments (
                PaymentID int PRIMARY key IDENTITY,
				PaymentDescription text,
                SenderFullName VARCHAR(70)  NOT NULL,
				SenderID int Foreign key references [User],
                RecieverFullName VARCHAR(70)  NOT NULL,
				RecieverID int Foreign key references [User],
                Amount DECIMAL(9,2)  NOT NULL,
                isSalary BIT DEFAULT(0),
                isProjectPaiement BIT DEFAULT(0),
                ProjectID int FOREIGN KEY REFERENCES Project,
                Date date  NOT NULL
            )

            create table Task (
                TaskID int PRIMARY key IDENTITY,
                Project int FOREIGN key REFERENCES Project  NOT NULL,
                Name VARCHAR(30)  NOT NULL,
                Description text  NOT NULL,
                Priority int  NOT NULL,
                Difficulty int  NOT NULL,
                DeadLine date  NOT NULL,
                Stack int FOREIGN key REFERENCES Stack,
                isComplete BIT DEFAULT(0),
                isBooked BIT DEFAULT (0),
                DoneBy int FOREIGN key REFERENCES [USER] DEFAULT(null)
            )

            Create table Issue (
                IssueID int PRIMARY key IDENTITY,
                Task int FOREIGN key REFERENCES Task  NOT NULL,
                Issuer int FOREIGN KEY REFERENCES [User]  NOT NULL,
                Title VARCHAR(30)  NOT NULL,
                Description text  NOT NULL,
                isSolved BIT DEFAULT(0)
            )

            Create TABLE ActionLog (
                ActionID int PRIMARY KEY IDENTITY NOT NULL,
                UserName VARCHAR(35) NOT NULL,
                UserFullName VARCHAR(60) NOT NULL,
                ActionName VARCHAR(35) NOT NULL,
                ActionMethod CHAR(8) NOT NULL,
                ActionDATA TEXT NOT NULL,
                RequestDate DATETIME DEFAULT(GETDATE()) NOT NULL
            )
            
            COMMIT Tran
	    Print 'DB and Tables Created Successfully, Creating Default Roles...'
	    insert into [Role](RoleName) values ('Manager'), ('TeamLeader'), ('Member'), ('Client')

        PRINT 'Fixing DB Collation'
        ALTER DATABASE ProjectManagerDB
        COLLATE Latin1_General_CS_AS        

        Print 'DB Completed Successfully'
        END TRY
        Begin catch 
            THROW
            -- SELECT  
            --     ERROR_NUMBER() AS ErrorNumber  
            --     ,ERROR_SEVERITY() AS ErrorSeverity  
            --     ,ERROR_STATE() AS ErrorState  
            --     ,ERROR_PROCEDURE() AS ErrorProcedure  
            --     ,ERROR_LINE() AS ErrorLine  
            --     ,ERROR_MESSAGE() AS ErrorMessage; 
            ROLLBACK
        End Catch

    END

    PRINT 'Creating Trigger on User Deletion'
    GO
    
    IF EXISTS (SELECT * FROM sys.objects WHERE type = 'TR' AND name = 'DisableAccount') 
    DROP TRIGGER DisableAccount
    GO

    CREATE TRIGGER DisableAccount
    ON [User]
    INSTEAD OF DELETE
    AS
    BEGIN
        UPDATE [User] SET isAccountActive = 0 WHERE UserID in (SELECT UserID from Deleted)
    END
