using DbUp;
using Microsoft.Extensions.Configuration;

const string databaseConnectionStringSection = "DatabaseConnectionString";
const string defaultConnection = "DefaultConnection";
const string appsettings = "appsettings.json";

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile(appsettings, optional: false, reloadOnChange: true)
    .Build();

var result =
    DeployChanges.To
        .PostgresqlDatabase(configuration.GetSection(databaseConnectionStringSection)[defaultConnection])
        .WithScriptsFromFileSystem("Scripts")
        .LogToConsole()
        .Build()
        .PerformUpgrade();