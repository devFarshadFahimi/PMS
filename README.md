# App_MicroService_Product

dotnet ef  migrations add "init" -p ./src/Infrastructure/Infrastructure.csproj -s ./src/WebUI/WebUI.csproj

dotnet ef database update -p ./src/Infrastructure/Infrastructure.csproj -s ./src/WebUI/WebUI.csproj --context ApplicationDbContext

Microservice Product : http://localhost:6100/swagger/index.html

docker run --rm -p 6100:6000 -p 6101:6001 -e ASPNETCORE_HTTP_PORT=https://+:6001 -e ASPNETCORE_URLS=http://+:6000 microservice-product


docker compuse build

docker compose up

Scaffold-DbContext "Name=ConnectionStrings:DefaultConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir QueryPersistence -force

	docker run --name seq -d -e ACCEPT_EULA=Y -e SEQ_FIRSTRUN_ADMINUSERNAME=admin -e SEQ_FIRSTRUN_ADMINPASSWORDHASH="123456aA@" -e 	SEQ_FIRSTRUN_REQUIREAUTHENTICATIONFORHTTPINGESTION=True -v C:/seq-data/data:/data -p 5341:80 --memory=16g --memory-swap=16g datalust/seq