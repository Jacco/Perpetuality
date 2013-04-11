CREATE FUNCTION [dbo].[GeneratePasswordHash]
(
	@Password nvarchar(256)
)
RETURNS binary(16)
AS
BEGIN
	declare @PasswordHashSecret varchar(256) = (select strValue from tblSetting where strName = 'PasswordHashSecret')
	DECLARE @result binary(16) = HASHBYTES('MD5', @Password + @PasswordHashSecret)
	RETURN @result
END
