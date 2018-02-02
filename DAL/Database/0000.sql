create database TaskDB
GO

use TaskDB
GO

	CREATE TABLE [dbo].[TaskHolder] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE MigrationHistory
(
    Id INT IDENTITY (1, 1) NOT NULL,
    CurrentVersion VARCHAR(2),
    FileNumber VARCHAR(4),
    Comment VARCHAR(255),
    DateApplied DATETIME,

    PRIMARY KEY(Id)
)
GO

INSERT INTO 
MigrationHistory ( CurrentVersion, FileNumber, Comment,    DateApplied )
VALUES           ( '00',         '0000',     'create DB and tables', GETDATE()   )
GO