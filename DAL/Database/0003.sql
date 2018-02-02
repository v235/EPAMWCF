use TaskDB
GO

ALTER TABLE "TaskHolder"
ADD [downloadPath] NVARCHAR (50) NULL
GO

INSERT INTO 
MigrationHistory ( CurrentVersion, FileNumber, Comment,    DateApplied )
VALUES           ( '03',         '0003',     '0003.sql', GETDATE()   )
GO

