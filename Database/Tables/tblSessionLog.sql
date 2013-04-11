CREATE TABLE [dbo].[tblSessionLog](
	[autID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[datCreated] [datetime] NOT NULL,
	[guidID] [uniqueidentifier] NOT NULL,
	[intUserRID] [bigint] NOT NULL,
	[strMessage] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tblSessionLog] PRIMARY KEY CLUSTERED 
(
	[autID] ASC
)
)

GO

ALTER TABLE [dbo].[tblSessionLog] ADD  CONSTRAINT [DF_tblSessionLog_datCreated]  DEFAULT (getdate()) FOR [datCreated]
GO
