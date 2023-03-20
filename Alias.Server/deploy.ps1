    cd C:\Users\kovac\OneDrive\Programare\projects\Alias\Alias.Client
ng build --configuration production
cd ..\Alias.Server
dotnet publish
cp -r C:\Users\kovac\OneDrive\Programare\projects\Alias\Alias.Client\dist\Alias.Client.* C:\Users\kovac\OneDrive\Programare\projects\Alias\Alias.Server\bin\Release\net6.0\publish\wwwroot