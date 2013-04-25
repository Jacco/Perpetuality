CREATE PROCEDURE [dbo].[ChangeUserPasswordInternal]
		@UserID bigint
	,	@Password nvarchar(256)	
	,	@DeactivateSessions bit = 1
AS
BEGIN
	SET NOCOUNT ON;

	declare @hash binary(16) = dbo.GeneratePasswordHash(@Password)
	
	if (@DeactivateSessions = 1)
	begin
		update tblPlayerSession set bitActive = 0 where intUserRID = @UserID
	end
	
	update tblPlayer set binPassword = @hash, datPasswordChanged = GETDATE() where autID = @userid
END
