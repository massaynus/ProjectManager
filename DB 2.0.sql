if not Exists(select name from master.dbo.sysdatabases where name ='ProjectManagerDB')
BEGIN
    use master
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
                Name VARCHAR(35)
            )

            create table Stack (
                StackID int PRIMARY key IDENTITY,
                Name VARCHAR(40),
                Tools text
            )

            create table TeamStack (
                Num int PRIMARY KEY IDENTITY,
                Team int FOREIGN key REFERENCES Team,
                Stack int FOREIGN key REFERENCES Stack
            )

            create table [Role](
                RoleID int PRIMARY key IDENTITY,
                RoleName VARCHAR(25)
            )

            create table [User] (
                UserID int PRIMARY KEY IDENTITY,
                UserName VARCHAR(35),
                Password VARCHAR(160),
                FirstName VARCHAR(35),
                LastName VARCHAR(35),
                BirthDtae DATE,
                GSM VARCHAR(15),
                Email VARCHAR(200),
                RIB VARCHAR(23),
                [Role] INT FOREIGN KEY REFERENCES [Role],
                Team INT FOREIGN key REFERENCES Team
            )

            CREATE TABLE [Address] (
                AddressID int PRIMARY key IDENTITY,
                UserID int FOREIGN key REFERENCES [User],
                street VARCHAR(120),
                ZipCode CHAR(8),
                City VARCHAR(20),
                State VARCHAR(20),
                Country VARCHAR(20) 
            )

            create table Project (
                ProjectID int primary key IDENTITY,
                Name VARCHAR(30),
                Description text,
                Owner int FOREIGN key REFERENCES [User],
                Team int FOREIGN key REFERENCES Team,
                Budget DECIMAL(9,2),
                Priority int,
                Complexity int,
                StartinDate date,
                DueDate date
            )

            CREATE TABLE Paiments (
                PaymentID int PRIMARY key IDENTITY,
                Project int FOREIGN KEY REFERENCES Project,
                Amount DECIMAL(9,2),
                Date date
            )

            create table Task (
                TaskID int PRIMARY key IDENTITY,
                Project int FOREIGN key REFERENCES Project,
                Name VARCHAR(30),
                Description text,
                Priority int,
                Difficulty int,
                DeadLine date,
                Stack int FOREIGN key REFERENCES Stack,
                isComplete BIT,
                DoneBy int FOREIGN key REFERENCES [USER] DEFAULT(null)
            )

            Create table Issue (
                IssueID int PRIMARY key IDENTITY,
                Task int FOREIGN key REFERENCES Task,
                Issuer int FOREIGN KEY REFERENCES [User],
                Title VARCHAR(30),
                Description text,
                isSolved BIT DEFAULT(0)
            )
            
            COMMIT Tran
	    Print 'DB and Tables Created Successfully, Creating Default Roles...'
	    insert into [Role](RoleName) values ('Manager'), ('TeamLeader'), ('Member'), ('Client')


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
