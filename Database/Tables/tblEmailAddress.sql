CREATE TABLE [dbo].[tblEmailAddress] (
    [autID]           BIGINT           IDENTITY (1, 1) NOT NULL,
    [strEmailAddress] NVARCHAR (256)   NOT NULL,
    [guidToken]       UNIQUEIDENTIFIER CONSTRAINT [DF_tblEmailAddress_guidToken] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_tblEmailAddress] PRIMARY KEY CLUSTERED ([autID] ASC)
);

