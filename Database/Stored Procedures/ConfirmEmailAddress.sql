CREATE PROCEDURE [dbo].[ConfirmEmailAddress]
	@Hash varchar(32)
AS
BEGIN
	SET NOCOUNT ON;

	declare @emailid bigint = null
	declare @confirmed bit
	declare @userid bigint
	
	if (select COUNT(*) from tblPlayerEmail where bitActive = 1 and IsNull(strConfirmHash, 'nohash_1') = @Hash) > 1
	begin
		raiserror('60010 Email activation failed', 16, 1)
		return 0	
	end
	
	select
			@emailid = autID
		,	@confirmed = bitConfirmed
		,	@userid = intPlayerID
	from
		tblPlayerEmail
	where
			bitActive = 1
		and IsNull(strConfirmHash, 'nohash_1') = @Hash
		
	if (@emailid is not null)
	begin
		if (@confirmed = 0)
		begin
			update
				tblPlayerEmail
			set
				bitConfirmed = 1, datConfirmed = GetDate()
			where
				autID = @emailid
				
			update
				tblPlayer
			set
				intPrimaryPlayerEmailID = @emailid
			where
				autID = @userid
				
			update
				tblPlayerEmail
			set
				bitActive = 0
			where
					intPlayerID = @userid
				and	autID <> @emailid
							
			return 1
		end else begin
			raiserror('60011 The email address in question was already confirmed.', 16, 1)
			return 0
		end
	end else begin
		raiserror('60010 Email activation failed', 16, 1)
		return 0
	end
END