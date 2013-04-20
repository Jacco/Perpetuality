CREATE PROCEDURE [dbo].[GetConfirmationHash]
	@EmailAddress nvarchar(265)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select distinct ue.strConfirmHash
	from tblPlayer u
		join tblPlayerEmail ue on ue.intPlayerID = u.autID
			join tblEmailAddress ea on ue.intEmailAddressID = ea.autID
	where 1=1
	and ue.bitConfirmed = 0
	and ue.bitActive = 1
	and ea.strEmailAddress = @EmailAddress
END