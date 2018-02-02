use TaskDB
GO

ALTER TABLE "TaskHolder"
ADD [status] NVARCHAR (10) NOT NULL
GO

INSERT INTO 
MigrationHistory ( CurrentVersion, FileNumber, Comment,    DateApplied )
VALUES           ( '02',         '0002',     '0002.sql', GETDATE()   )
GO

