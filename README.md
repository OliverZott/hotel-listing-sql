# Hotel Listing - API Example

- ASP.NET
- PostgreSQL
- EF Core
- Serilog / Seq
- Automapper
- Security - Identity (EF Core)
- Logging
- Global Exception Handling

- Pattern:
  - Repository Pattern / Unit Of Work
  - DTO

## Run App

Check if Seq and PostgreSQL services are running

```shell
.\CheckAndStartService.ps1
```

Update/Create Database (in PackageManagerConsole)

```shell
Update-Database
```

Start app

```shell
dotnet run
```

Endpoints:

- Seq: <http://localhost:5341/#/events>
- App: <https://localhost:7170/swagger/index.html>

## Debugging

If debugging not possible:

- Select "Enable native code debugging" [Link](https://docs.microsoft.com/en-us/visualstudio/debugger/how-to-enable-debugging-for-aspnet-applications?view=vs-2022)

## Steps

- Create Project and Solution `ASP.NET Core Web API`
- **CORS** configuration
- **Serilog** and **Seq**
  - **ps script** for checking and starting mongodb and seq service
- PostgreSQL setup/config
  - Entity Framework Core
  - Driver package
- Services & Controllers
  - Create Services and **register** in `Programm.cs` to DI Container
  - **Inject** service in Controller
  - **Logging** and **ExceptionHandling** in Service/Controller
- **Scaffold API-Controller**
  - add `API Controller with actions, using Entity Framework`
  - select Entity class (e.g. Hotel or Country -- NOT Dtos!)
- Refactor:
  - Repository Pattern / Unit Of Work (separate context from controller)
  - DTO (separate entities from andpoint)
- Security
  - Identity EF Core

# ---

## Serilog

- [Tutorial 1](https://www.youtube.com/watch?v=MYKTwvowMUI)
- [Tutorial 2](https://www.youtube.com/watch?v=hJ0QHRV3RPQ) - <https://github.com/rstropek/htl-leo-pro-5/tree/master/lectures/0500-api-error-handling/WebApiErrorHandling.Server>
- [Tutorial 3](https://www.youtube.com/watch?v=_iryZxv8Rxw)

## Seq

- **Install** seq and use **Sink** in `appsettings.json`
- Using RequestID in search to find all request related stuff: `RequestId = "0HMHQ499O1RPJ:0000001B"`

## Database connection

- Install packages
- ConnectionString / DBContext / Service registration
- Create Entities
- **Add Migration** and **Update Database**
  - `add-migration <InitialMigration>`
  - `update-database`

## Scaffold API

- start with entity with fewest Foreign keys!

## Environments

- <https://www.youtube.com/watch?v=_2_qksdQKCE>
- <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0>

## PostgreSQL

- `choco install postgresql`
- `choco install pgadmin4`

### Setup User

[Chocolatey Software | PostgreSQL 14.4.1](https://community.chocolatey.org/packages/postgresql "‌")

- If you didn't specify password during setup and didn't record the generated one, you need manually reset it using the following steps:
  - Open file `data\pg_hba.conf` in PostgreSql installation directory
  - Change `METHOD` to `trust` and restart service with `Restart-Service postgresql*`
  - Execute `"alter user postgres with password 'password';" | psql -Upostgres`
  - Revert back `data\pg_hba.conf` to METHOD `md5` and restart service

### Use CLI

Linux:

```shell
sudo -u postgres psql
```

``` shell
psql -V
psql -U postgres
\q
```

Show databases, connect to database and show tables/info/users/query:

```shell
\l
\c <dbName>
\d
\dt
\du
SELECT * FROM "<tableName>";
```

## Security

- Check Token at <https://jwt.io/>
- Customize response messages:
  - <https://referbruv.com/blog/building-custom-responses-for-unauthorized-requests-in-aspnet-core/>

## Logging

- Mainly in case of errors
- Client must not see Backend implementation specifics

## Global Exception Handling

- Throw custom exceptions
- Hijack http requests and handle exceptions
  - logging
  - http repsonses
- Using middleware, registered in DI Container

## API Versioning

Example in CountriesControlerV2

- 3 ways:
  - request param in url
  - header param
  - part of api url
- `Microsoft.AspNetCore.Mvc.Versioning`
- `Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer`
- Service configuration in `Program.cs`

## OData

Example in CountriesControlerV2

- Filter, Query, Order Controller-Responses

### Query

- Backend: `[EnableQuery]` at Controller
- Request: Query Parameter `$select=`

## TODO

- remove unnecessary/specific schemas from swagger
