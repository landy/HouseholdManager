open System.IO
open DbUp
open Microsoft.Extensions.Configuration

let configBuilder =
    ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true)
        .AddEnvironmentVariables()

let configuration = configBuilder.Build()

let upgradeEngine =
    DeployChanges.To
        .PostgresqlDatabase(configuration.GetConnectionString("householddb"))
        .WithScriptsFromFileSystem("./scripts")
        .LogToConsole()
        .Build()

upgradeEngine.PerformUpgrade() |> ignore