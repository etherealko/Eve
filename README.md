# Eve
A telegram bot project.

Plugin-based C# telegram bot with weird code samples.

# How to start this

1. Set up SQL database instance. If you are using VS included SQLEXPRESS the connection string will be like @"data source=(localDb)\MSSQLLocalDB;initial catalog=EveDb;integrated security=True;MultipleActiveResultSets=True" 
2. Change the db deploy connection string at eth.MacTestApp.EveDbFactory.
3. Open commandline (or powershell) at eth.Eve.Storage project root folder and run  
	 dotnet restore  
	 dotnet ef -p . -s ../eth.MacTestApp database update
4. Inject your connection string into your bot instance.
5. Most likely everything should work now.

# MySQL and other non MSSQL dbms

I made some experiments under the macOS environment with MySQL using third party EF adapter, after some polishing this definitely should be usable, but you (me) should use different migration sets for each of the supported dbms. This will be done at some point in time.

Another important point is that the default db option should definitely be something like Sqlite, not a goddamn MSSQL.
