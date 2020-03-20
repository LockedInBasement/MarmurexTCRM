CREATE PROCEDURE [dbo].[spProduct_GetById]
	@ID int
AS
begin
set nocount on;

	select [Id], [ProductName],[Description], [RetailPrice], [QuantityInStock], [IsTaxable]
	from dbo.Product
	where Id = @ID;
end
