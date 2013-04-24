CREATE PROCEDURE [dbo].[UpdatePlayerBalance]
		@PlayerID bigint
	,	@WorldID bigint
	,	@Balance numeric(18,2) output
AS
BEGIN
	declare @numBalance numeric(18, 2)
	declare @lastUpdated datetime
	declare @numCreditProductionrate numeric(18, 5)

	select
			@numBalance = numBalance
		,	@lastUpdated = datBalanceUpdated
		,	@numCreditProductionrate = numCreditProductionRate
	from
		tblWorldPlayer
	where
			intPlayerID = @PlayerID
		and intWorldID = @WorldID

	declare @newDate datetime = GetDate()
	declare @gameSecondsSinceLastUpdate numeric(18, 6) = 365.0 * datediff(millisecond, @lastUpdated, @newDate) / 1000

	update tblWorldPlayer
	set
			numBalance = numBalance + @gameSecondsSinceLastUpdate * @numCreditProductionrate
		,	datBalanceUpdated = @newDate
	where
			intPlayerID = @PlayerID
		and intWorldID = @WorldID

	set @Balance = @numBalance + @gameSecondsSinceLastUpdate * @numCreditProductionrate
	RETURN 0
END
