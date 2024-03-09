open System
open Dapper
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open System.Data

open HouseholdManager.Cookbook.Ports
open HouseholdManager.Cookbook
open HouseholdManager.Adapter.API
open HouseholdManager.Adapter.PostgreSQL

let private setupServices
    (settings: ConfigurationManager)
    (services: IServiceCollection)
    =
    DefaultTypeMap.MatchNamesWithUnderscores <- true

    services.AddScoped<UseCase.ListRecipes>(
        Func<IServiceProvider, UseCase.ListRecipes>(fun sc ->
            let loader = sc.GetRequiredService<RecipesListLoader>()
            fun () -> ListRecipesUseCase.execute loader)
    )
    |> ignore

    services.AddScoped<UseCase.ListRecipes>(
        Func<IServiceProvider, UseCase.ListRecipes>(fun sc ->
            let loader = sc.GetRequiredService<RecipesListLoader>()
            fun () -> ListRecipesUseCase.execute loader)
    )
    |> ignore

    services.AddScoped<UseCase.ListRecipes>(
        Func<IServiceProvider, UseCase.ListRecipes>(fun sc ->
            let loader = sc.GetRequiredService<RecipesListLoader>()
            fun () -> ListRecipesUseCase.execute loader)
    )
    |> ignore

    services.AddScoped<IDbConnection>(
        Func<IServiceProvider, IDbConnection>(fun sc ->
            let connString = settings.GetConnectionString("householddb")
            new Npgsql.NpgsqlConnection(connString))
    )
    |> ignore

    services.AddScoped<RecipesListLoader>(
        Func<IServiceProvider, RecipesListLoader>(fun sc ->
            let conn = sc.GetRequiredService<IDbConnection>()
            Cookbook.Recipes.loadRecipes conn)
    )
    |> ignore

[<EntryPoint>]
let main args = Host.run args setupServices