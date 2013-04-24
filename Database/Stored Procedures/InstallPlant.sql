CREATE PROCEDURE [dbo].[InstallPlant]
		@PlayerID bigint
	,	@WorldID bigint
	,	@PowerPlantTypeID bigint
	,	@Longitude numeric(18, 6)
	,	@Latitude numeric(18, 6)
	,	@InstallationSize int
	,	@SolarPower numeric(18,2)

	,	@Balance numeric(18, 2) output
	,	@CreditProductionRate numeric(18,6) output
	,	@GameDate datetime output
AS
BEGIN
	-- update the player balance
	exec GetPlayerState @PlayerID, @WorldID, @Balance output, @CreditProductionRate output, @GameDate output

	-- calculate the panel
	declare @pwr numeric(18,6)
	declare @cst numeric(18,2)
	declare @rev numeric(18,6)

	exec CalculateSolarPanel @InstallationSize, @Longitude, @Latitude, @GameDate, @SolarPower, @pwr output, @cst output, @rev output

	select @pwr, @cst, @rev

	if (@Balance > @cst)
	begin
		-- pay for the panel
		update tblWorldPlayer
		set 
				numBalance = numBalance - @cst
			,	numCreditProductionRate = numCreditProductionRate + @rev
		where
				intPlayerID = @PlayerID
			and	intWorldID = @WorldID

		-- store the plant
		insert into tblWorldPlayerPlant(intWorldID, intPlayerID, intPowerPlantTypeID, numLongitude, numLatitude, intInstalationSize)
		select @WorldID, @PlayerID, @PowerPlantTypeID, @Longitude, @Latitude, @InstallationSize

		exec GetPlayerState @PlayerID, @WorldID, @Balance output, @CreditProductionRate output, @GameDate output
		return 0
	end
	exec GetPlayerState @PlayerID, @WorldID, @Balance output, @CreditProductionRate output, @GameDate output
	return 1
END
