dotnet clean
dotnet build
$a = Get-Date -Format "yyyyMMddHHHmmss"
dotnet ef migrations remove
dotnet ef migrations add $a
dotnet ef database update