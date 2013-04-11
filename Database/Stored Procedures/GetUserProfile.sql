CREATE PROCEDURE [dbo].[GetUserProfile]
		@Session uniqueidentifier
	,	@IPAddress varchar(22)
AS
BEGIN
	SET NOCOUNT ON;

	declare @result int
	declare @UserID bigint
	
	exec @result = ValidateSession @Session = @Session, @IPAddress = @IPAddress, @Reason = 'GetUserProfile', @UserID = @UserID output
	
	if (@result <> 0 and @UserID is not null)
	begin
		select
				u.autID
			,	u.datCreated
			,	ea.strEmailAddress
			,	u.strName
		from
			tblPlayer	u join tblPlayerEmail ue on u.intPrimaryPlayerEmailID = ue.autID
				join tblEmailAddress ea on ue.intEmailAddressID = ea.autID
		where
			u.autID = @UserID
			
		return 1
	end else begin
 		raiserror('60101 Supplied session token did not refer to an active session.', 16, 1)
		return 0	
	end
END
