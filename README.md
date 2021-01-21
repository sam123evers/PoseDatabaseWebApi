# PoseDatabaseWebApi
.NET Core Web API for Pose Database

 ## Build app in Release mode and run locally
 ### it will be connected to Azure PostgreSQL db aka using appsettings.json
 solution config: debug
 debug menu: release
 uncommented DB Migration method in Startup.cs
 
 ## Build app in Debug mode and debug locally
 ### it will be connected to local PostgreSQL instance aka using appsettings.Dev.json
 solution config: debug
 debug menu: IIS Express
 COMMENTED DB Migration method in startup.cs
 leaving Migrate() commented throws this error:
 Table 'Poses' already exists
