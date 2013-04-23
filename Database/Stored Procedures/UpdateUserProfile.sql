CREATE PROCEDURE [dbo].[UpdateUserProfile]
	@Session uniqueidentifier,	
	@IPAddress varchar(22),
	@Name nvarchar(256),
	@Language char(2)
AS
BEGIN
	SET NOCOUNT ON;

	declare @result int
	declare @UserID bigint
	
	exec @result = ValidateSession @Session = @Session, @IPAddress = @IPAddress, @Reason = 'UpdateUserProfile', @UserID = @UserID output
	
	if (@result <> 0 and @UserID is not null)
	begin
		update
			tblPlayer
		set
				strName	= @Name,
				strLanguage = @Language
		where
			autID = @UserID
		return 1
	end else begin
 		raiserror('Supplied session token did not refer to an active session.', 16, 1)
		return 0	
	end
END
