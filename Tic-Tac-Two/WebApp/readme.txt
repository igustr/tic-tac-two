dotnet aspnet-codegenerator razorpage -m Game -outDir Pages/Games -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Configuration -outDir Pages/Configurations -dc AppDbContext -udl --referenceScriptLibraries

dotnet ef migrations add InitialMigration --project "DAL/DAL.csproj"
dotnet ef database update --project "DAL/DAL.csproj"

