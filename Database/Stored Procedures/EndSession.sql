CREATE PROCEDURE [dbo].[EndSession]
		@Session uniqueidentifier
	,	@IPAddress varchar(22)
AS
BEGIN
	SET NOCOUNT ON;

	if (exists(select * from tblPlayerSession where guidID = @Session and strIPAddress = @IPAddress and bitActive = 1))
	begin
		exec ValidateSession @Session = @Session, @IPAddress = @IPAddress, @Reason = 'Logout' 		
		
		update
			tblPlayerSession
		set
			bitActive = 0
		where
			guidID = @Session
			
		return 1
	end else begin
		raiserror('60032 Supplied session token did not refer to an active session.', 16, 1)
		return 0
	end
END