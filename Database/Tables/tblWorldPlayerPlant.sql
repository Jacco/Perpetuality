CREATE TABLE [dbo].[tblWorldPlayerPlant]
(
	[autID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [intWorldID] BIGINT NOT NULL, 
    [intPlayerID] BIGINT NOT NULL, 
    [intPowerPlantTypeID] BIGINT NOT NULL, 
    [numLongitude] NUMERIC(18, 6) NOT NULL, 
    [numLatitude] NUMERIC(18, 6) NOT NULL, 
    [intInstalationSize] INT NOT NULL
)
