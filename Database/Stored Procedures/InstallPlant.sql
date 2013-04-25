CREATE PROCEDURE [dbo].[InstallPlant]
		@PlayerID bigint
	,	@WorldID bigint
	,	@PowerPlantTypeID bigint
	,	@Longitude numeric(18, 6)
	,	@Latitude numeric(18, 6)
	,	@InstallationSize int
	,	@SolarPower numeric(18,2)
	,	@CalculateOnly bit

	-- player state returned
	,	@Balance numeric(18, 2) output
	,	@CreditProductionRate numeric(18,6) output
	,	@GameDate datetime output
	,	@InstalledPower numeric(18,2) output
	-- building stats returned
	,	@BuildingCost numeric(18, 2) output
	,	@BuildingPower numeric(18,2) output
	,	@CreditRevenuePerYear numeric(18,2) output
AS
BEGIN
	set nocount on

	-- update the player balance
	exec GetPlayerState @PlayerID, @WorldID, @Balance output, @CreditProductionRate output, @GameDate output, @InstalledPower output

	exec CalculateSolarPanel @InstallationSize, @Longitude, @Latitude, @GameDate, @SolarPower, @BuildingPower output, @BuildingCost output, @CreditRevenuePerYear output

	if (@Balance > @BuildingCost and @CalculateOnly = 0)
	begin
		-- pay for the panel
		update tblWorldPlayer
		set 
				numBalance = numBalance - @BuildingCost
			,	numCreditProductionRate = numCreditProductionRate + @CreditRevenuePerYear / @BuildingCost
			,	numInstalledPower = numInstalledPower + @BuildingPower
		where
				intPlayerID = @PlayerID
			and	intWorldID = @WorldID

		-- store the plant
		insert into tblWorldPlayerPlant(intWorldID, intPlayerID, intPowerPlantTypeID, numLongitude, numLatitude, intInstalationSize)
		select @WorldID, @PlayerID, @PowerPlantTypeID, @Longitude, @Latitude, @InstallationSize

		exec GetPlayerState @PlayerID, @WorldID, @Balance output, @CreditProductionRate output, @GameDate output, @InstalledPower output
		return 0
	end
	exec GetPlayerState @PlayerID, @WorldID, @Balance output, @CreditProductionRate output, @GameDate output, @InstalledPower output
	return 1
END
