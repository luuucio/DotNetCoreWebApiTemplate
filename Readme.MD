# .NET Core 3.1 Web API Template

A simple template for a Web API project, using Entity Framework Core, Automapper and several best practices.  
## Goals
We will create an API for handling a Student entity in a SQL Server database, with a slightly different model in the endpoints.

The entity in the database is the following:

```sql
Student
    Id          INT
    FirstName   VARCHAR
    LastName    VARCHAR
    Email       VARCHAR
```

The model in the API will be:

```csharp
Student
    Id          int
    Name        string // merge FirstName and LastName
    Email       string
```

The rationale for the difference is: in an API we should strive to think about a model, which can be different from the entity in a database.  
In this simple example a slight difference is used to show how to use Automapper.

## Setup

1) Create a new .NET Core API project and delete the WeatherForecast classes if present.
2) Install from Nuget:    
    - Microsoft.EntityFrameworkCore.SqlServer
    - Microsoft.EntityFrameworkCore.Design
    - AutoMapper.Extensions.Microsoft.DependencyInjection
   
## Entity
Create the Entities folder and create the `Student` class. Note the `[Required]` attribute, the optional property `Phone` through the use of `string?` and the pragma `#nullable`

## DbContext
Create the DbContexts folder and add the `WebApiTemplateContext` class, which will inherit from `DbContext`.  
The `OnModelCreating` method is used to create an index on the LastName field (for demo purpose).
A constructor is needed to receive the configuration specified in the `Startup.cs` file.

Next, add the DbContext
```csharp
services.AddDbContext<CourseLibraryContext>(options =>
{
    options.UseSqlServer(connection); // Defined in appsettings.json
}); 
```
Then, add the connection string to appsettings.json
```json
"ConnectionString": "Server=(localdb)\\mssqllocaldb;Database=WebApiTemplateDB;Trusted_Connection=True;"
```

## Migration
Migrations are the recommended way to evolve the database schema using a code first approach.  
We will use the command line with [dotnet-ef](https://www.nuget.org/packages/dotnet-ef/)

```
C:\source\repos\DotNetCoreWebApiTemplate\DotNetCoreWebApiTemplate>dotnet ef

                     _/\__
               ---==/    \\
         ___  ___   |.    \|\
        | __|| __|  |  )   \\\
        | _| | _|   \_/ |  //|\\
        |___||_|       /   \\\/\\

Entity Framework Core .NET Command-line Tools 3.1.1

Usage: dotnet ef [options] [command]

Options:
  --version        Show version information
  -h|--help        Show help information
  -v|--verbose     Show verbose output.
  --no-color       Don't colorize output.
  --prefix-output  Prefix output with level.

Commands:
  database    Commands to manage the database.
  dbcontext   Commands to manage DbContext types.
  migrations  Commands to manage migrations.

Use "dotnet ef [command] --help" for more information about a command.
```

First we create a migration:

```
C:\source\repos\DotNetCoreWebApiTemplate\DotNetCoreWebApiTemplate>dotnet ef migrations add InitialCreate
Build started...
Build succeeded.

Done. To undo this action, use 'ef migrations remove'
```

After the completion we will find the Migrations folder in the project, which contains a file named like `XXXXXXXXXXXXXX_InitialCreate.cs` with the operations to perform on the database.  
It is strongly suggested to review the generated code to ensure the correctness.  

Next we will perform the migration:

```
C:\source\repos\DotNetCoreWebApiTemplate\DotNetCoreWebApiTemplate>dotnet ef database update
Build started...
Build succeeded.

Done.
```

After that, if we open the `WebApiTemplateDB.mdf` we should find the Persons table, with a primary key, a nullable column and an index on `LastName`:

```sql
CREATE TABLE [dbo].[Students] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (MAX) NOT NULL,
    [LastName]  NVARCHAR (450) NOT NULL,
    [Email]     NVARCHAR (MAX) NOT NULL,
    [Phone]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Students_LastName]
    ON [dbo].[Students]([LastName] ASC);
```
## Model
Create the Models folder and the `StudentDto` class. In this model we merged the first and last name in a single property and we removed the phone information.

## Repository
Create the Services folder, in which we will put the `IWebApiTemplateRepository` interface and the `WebApiTemplateRepository` class.

## Automapper
Create the `Profiles` folder with the `PersonProfile` class.

## Controller
In the `Controller` folder add the PersonController class.  
**Do not add a Controller, we will use an empty class and build the controller ourselves.**  

Notes:  
- The `[ApiController]` attribute gives us some nice behavior, such as automatic HTTP 400 responses, binding source parameter inference, etc.
- We inherit from `ControllerBase`, because inheriting from `Controller` would give us Views support, which is out of scope on a web api project
- A constructor is required to inject the Repository and the Mapper.

## Startup.cs
In the `ConfigureServices` method some changes are necessary.
1) If the client requests a response format not supported we want to return an error 
2) Our application responds only with JSON, we want to add XML

The code is the following:

```csharp
services.AddControllers(setupAction =>
    {
        setupAction.ReturnHttpNotAcceptable = true; // Point 1
    }).AddXmlDataContractSerializerFormatters();   // Point 2
```
3) Add Automapper
```csharp
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
```
4) Add the repository
```csharp
services.AddScoped<IWebApiTemplateRepository, WebApiTemplateRepository>();
```


