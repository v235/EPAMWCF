use TaskDB
GO

ALTER TABLE "TaskHolder"
ADD [url] NVARCHAR (50) NOT NULL
GO


INSERT INTO 
MigrationHistory ( CurrentVersion, FileNumber, Comment,    DateApplied )
VALUES           ( '01',         '0001',     '0001.sql', GETDATE()   )
GO

