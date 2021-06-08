dotnet ef database drop -c UserContext
dotnet ef migrations remove -c UserContext 
dotnet ef migrations add Init -c UserContext -o ./Core/Data/Identity/Migrations
dotnet ef database update -c UserContext
# dotnet watch run