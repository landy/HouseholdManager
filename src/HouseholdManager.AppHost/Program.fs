module HouseholdManager.AppHost.App

open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection

open HouseholdManager.Adapter.API
open HouseholdManager.AppHost

let private setupServices
    (settings: ConfigurationManager)
    (services: IServiceCollection)
    =

    let connectionString = settings.GetConnectionString("householddb")

    Adapters.PostgreSQL.configureServices connectionString services
    |> Cookbook.configureServices
    |> ignore


type Program() =
    static member Run args =
        let api = Host.HouseholdManagerApi()
        api.Run args setupServices


[<EntryPoint>]
let main args = Program.Run args