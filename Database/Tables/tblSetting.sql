CREATE TABLE [dbo].[tblSetting]
(
	[autID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [strName] VARCHAR(256) NOT NULL, 
    [strValue] NVARCHAR(MAX) NULL
)
