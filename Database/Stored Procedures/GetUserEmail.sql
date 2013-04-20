CREATE PROCEDURE [dbo].[GetUserEmail]
		@UserID bigint
	,	@Email nvarchar(256) output
AS
BEGIN
	SET NOCOUNT ON;

	set @Email = null

	SELECT 
		@Email = a.strEmailAddress
	FROM
		tblPlayer u join tblPlayerEmail e on e.autID = u.intPrimaryPlayerEmailID
			join tblEmailAddress a on e.intEmailAddressID = a.autID
	WHERE
		u.autID = @UserID
		
	RETURN 1
END