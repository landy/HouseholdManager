open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection

open HouseholdManager.Adapter.API

let private setupServices
    (settings: ConfigurationManager)
    (services: IServiceCollection)
    =

    let connectionString = settings.GetConnectionString("householddb")

    HouseholdManager.AppHost.Adapters.PostgreSQL.configureServices
        connectionString
        services
    |> HouseholdManager.AppHost.Cookbook.configureServices
    |> ignore




[<EntryPoint>]
let main args = Host.run args setupServices