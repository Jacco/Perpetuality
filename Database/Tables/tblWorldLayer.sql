CREATE TABLE [dbo].[tblWorldLayer] (
    [autID]      BIGINT IDENTITY (1, 1) NOT NULL,
    [intWorldID] BIGINT NOT NULL,
    [intLayerID] BIGINT NOT NULL,
    CONSTRAINT [PK_tblWorldLayer] PRIMARY KEY CLUSTERED ([autID] ASC)
);

