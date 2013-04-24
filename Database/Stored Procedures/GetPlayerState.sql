CREATE PROCEDURE [dbo].[GetPlayerState]
		@PlayerID bigint
	,	@WorldID bigint

	,	@Balance numeric(18, 2) output
	,	@CreditProductionRate numeric(18,6) output
	,	@GameDate datetime output
AS
BEGIN
	if not exists(select * from tblWorldPlayer where intPlayerID = @PlayerID and intWorldID = @WorldID)
	begin
		insert into tblWorldPlayer(intPlayerID, intWorldID, numBalance) 
		select @PlayerID, @WorldID, 30000000
	end

	exec UpdatePlayerBalance @PlayerID, @WorldID, @Balance output, @CreditProductionrate output

	declare @created datetime

	select
		@created = datCreated 
	from
		tblWorldPlayer
	where
			intWorldID = @WorldID
		and intPlayerID = @PlayerID

	set @GameDate = dateadd(second, 365 * datediff(second, @created, GetDate()), convert(date, '20130420', 112))
END