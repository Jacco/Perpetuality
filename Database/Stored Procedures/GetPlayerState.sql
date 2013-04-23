CREATE PROCEDURE [dbo].[GetPlayerState]
		@PlayerID bigint
	,	@WorldID bigint
AS
BEGIN
	if not exists(select * from tblWorldPlayer where intPlayerID = @PlayerID and intWorldID = @WorldID)
	begin
		insert into tblWorldPlayer(intPlayerID, intWorldID, numBalance) 
		select @PlayerID, @WorldID, 30000000
	end

	select
		numBalance
	,	numCreditProductionRate
	from
		tblWorldPlayer
END