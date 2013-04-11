CREATE TABLE [dbo].[tblWorld] (
    [autID]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [datCreated] DATETIME       CONSTRAINT [DF_tblWorld_datCreated] DEFAULT (getdate()) NOT NULL,
    [strName]    NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_tblWorld] PRIMARY KEY CLUSTERED ([autID] ASC)
);

