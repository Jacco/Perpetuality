CREATE TABLE [dbo].[tblPlayerSession](
	[autID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[guidID] [uniqueidentifier] NOT NULL,
	[datCreated] [datetime] NOT NULL,
	[intUserRID] [bigint] NOT NULL,
	[bitActive] [bit] NOT NULL,
	[strIPAddress] [varchar](22) NOT NULL,
	[datLastValidated] [datetime] NOT NULL,
	 CONSTRAINT [PK_tblPlayerSession_1] PRIMARY KEY CLUSTERED ([autID] ASC)
);
GO

ALTER TABLE [dbo].[tblPlayerSession] ADD  CONSTRAINT [DF_tblPlayerSession_datCreated]  DEFAULT (getdate()) FOR [datCreated]
GO

ALTER TABLE [dbo].[tblPlayerSession] ADD  CONSTRAINT [DF_tblPlayerSession_bitActive]  DEFAULT ((1)) FOR [bitActive]
GO

ALTER TABLE [dbo].[tblPlayerSession] ADD  CONSTRAINT [DF_tblPlayerSession_strIPAddress]  DEFAULT ('0.0.0.0') FOR [strIPAddress]
GO

ALTER TABLE [dbo].[tblPlayerSession] ADD  CONSTRAINT [DF_tblPlayerSession_datLastValidated]  DEFAULT (getdate()) FOR [datLastValidated]
GO
