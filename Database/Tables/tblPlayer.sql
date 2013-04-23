CREATE TABLE [dbo].[tblPlayer] (
    [autID]                   BIGINT           IDENTITY (1, 1) NOT NULL,
    [datCreated]              DATETIME         CONSTRAINT [DF_tblPlayer_datCreated] DEFAULT (getdate()) NOT NULL,
    [strName]                 NVARCHAR (256)   NULL,
    [intPrimaryPlayerEmailID] BIGINT           NULL,
    [binPassword]             BINARY (16)      NULL,
    [datPasswordChanged]      DATETIME         NULL,
    [guidUserReference]       UNIQUEIDENTIFIER CONSTRAINT [DF_tblPlayer_guidUserReference] DEFAULT (newid()) NOT NULL,
    [strLanguage] CHAR(2) NOT NULL DEFAULT 'en', 
    CONSTRAINT [PK_tblPlayer] PRIMARY KEY CLUSTERED ([autID] ASC)
);

