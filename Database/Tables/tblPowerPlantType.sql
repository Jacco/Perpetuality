CREATE TABLE [dbo].[tblPowerPlantType] (
    [autID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [strName] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_tblPowerPlantType] PRIMARY KEY CLUSTERED ([autID] ASC)
);

