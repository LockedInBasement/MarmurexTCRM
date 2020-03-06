CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
begin
	set nocount on;

	SELECT FirstName, LastName, EmailAddress, CreatedDate
	from [dbo].[User]
	Where Id = @Id;
end