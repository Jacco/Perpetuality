CREATE TABLE [dbo].[tblWorldPlayer] (
    [autID]       BIGINT   IDENTITY (1, 1) NOT NULL,
    [datCreated]  DATETIME CONSTRAINT [DF_tblWorldPlayer_datCreated] DEFAULT (getdate()) NOT NULL,
    [intWorldID]  BIGINT   NOT NULL,
    [intPlayerID] BIGINT   NOT NULL,
    CONSTRAINT [PK_tblWorldPlayer] PRIMARY KEY CLUSTERED ([autID] ASC)
);

