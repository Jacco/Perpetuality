CREATE PROCEDURE [dbo].[ValidateSession]
		@Session uniqueidentifier
	,	@IPAddress varchar(22)
	,	@Reason nvarchar(max)
	,	@UserID bigint = null output
AS
BEGIN
	SET NOCOUNT ON;

	set @UserID = null
	
	select 
		@UserID = intUserRID
	from
		tblPlayerSession
	where
			guidID = @Session
		and	strIPAddress = @IPAddress
		and bitActive = 1
		
	if (@UserID is not null)
	begin
		insert into tblSessionLog(guidID, intUserRID, strMessage) values (@Session, @UserID, @Reason)
		update tblPlayerSession set datLastValidated = GETDATE() where guidID = @Session
		return 1
	end else begin
		return 0
	end
END
