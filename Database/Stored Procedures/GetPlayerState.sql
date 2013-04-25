CREATE PROCEDURE [dbo].[GetPlayerState]
		@PlayerID bigint
	,	@WorldID bigint

	,	@Balance numeric(18, 2) = null output
	,	@CreditProductionRate numeric(18,6) = null output
	,	@GameDate datetime = null output
	,	@InstalledPower numeric(18,2) = null output
AS
BEGIN
	set nocount on

	if not exists(select * from tblWorldPlayer where intPlayerID = @PlayerID and intWorldID = @WorldID)
	begin
		insert into tblWorldPlayer(intPlayerID, intWorldID, numBalance) 
		select @PlayerID, @WorldID, 30000000
	end

	exec UpdatePlayerBalance @PlayerID, @WorldID, @Balance output

	declare @created datetime

	select	
		top 1 
				@created = datCreated 
			,	@CreditProductionRate = numCreditProductionRate
			,	@InstalledPower = numInstalledPower
	from 
		tblWorldPlayer 
	where 
			intWorldID = @WorldID 
		and intPlayerID = @PlayerID

	set @GameDate = dateadd(second, 365 * datediff(second, @created, GetDate()), convert(datetime, '20130420', 112))
	return 0
END