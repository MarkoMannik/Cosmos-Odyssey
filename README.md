Up Time assignment - solution by Marko Männik

# Cosmos-Odyssey:
.NET Core 3.1 application (Web)

# Cosmos-Odyssey.Tests:
Test for Cosmos-Odyssey project

## Configuration
* Appsettings.json - app. configuration, database connection etc.

* Prerequisities
    * Install dotnet core 3.1 SDK latest version.
	* MSSQL database
    * Use Visual studio 2019 or newer to build/run.
    * For how to use specific dotnet cli commands take a look at <https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet.>

* Nuget packages
    * Use `dotnet restore` in project folder to restore Nuget packages for specific project. Visual studio updates Nuget packages automatically automatically on first build.
	
* Building
    * Use Visual Studio to build solution or use dotnet command `dotnet build` in project folder where csproj file is.

* Running projects
    * Use Visual Studio to start speific project or use dotnet command `dotnet run` in project folder csproj file is.

* Running unit tests
    * Use Visual Studio to run all unittests at once or use dotnet command  `dotnet test` in a `.Test` project folder. Projects named `.Functest` are meant to run against specific (dev) datbase in azure and may give false negative with other databases.

