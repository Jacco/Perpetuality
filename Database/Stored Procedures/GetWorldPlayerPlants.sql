CREATE PROCEDURE [dbo].[GetWorldPlayerPlants]
		@PlayerID bigint
	,	@WorldID bigint
	,	@MinLongitude numeric(18, 6)
	,	@MaxLongitude numeric(18, 6)
	,	@MinLatutude numeric(18, 6)
	,	@MaxLatitude numeric(18, 6)
AS
BEGIN
	select
			autID 
		,	numLongitude
		,	numLatitude
		,	intPowerPlantTypeID
	from
		tblWorldPlayerPlant
	where
			intPlayerID = @PlayerID
		and intWorldID = @WorldID
		and numLongitude between @MinLongitude and @MaxLongitude
		and numLatitude between @MinLatutude and @MaxLatitude
	RETURN 0
END
