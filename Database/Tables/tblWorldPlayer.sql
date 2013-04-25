CREATE TABLE [dbo].[tblWorldPlayer] (
    [autID]       BIGINT   IDENTITY (1, 1) NOT NULL,
    [datCreated]  DATETIME CONSTRAINT [DF_tblWorldPlayer_datCreated] DEFAULT (getdate()) NOT NULL,
    [intWorldID]  BIGINT   NOT NULL,
    [intPlayerID] BIGINT   NOT NULL,
    [numBalance] NUMERIC(18, 2) NOT NULL DEFAULT (0), 
    [datBalanceUpdated] DATETIME NOT NULL DEFAULT (getdate()), 
    [numCreditProductionRate] NUMERIC(18, 5) NOT NULL DEFAULT (0), 
    [numInstalledPower] NUMERIC(18, 2) NOT NULL DEFAULT (0), 
    CONSTRAINT [PK_tblWorldPlayer] PRIMARY KEY CLUSTERED ([autID] ASC)
);

