module HouseholdManager.Adapter.API.Host

open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

open HouseholdManager.Cookbook.Ports

let run
    (args: string[])
    (setupServices: ConfigurationManager -> IServiceCollection -> unit)
    =
    let builder = WebApplication.CreateBuilder(args)

    setupServices builder.Configuration builder.Services

    let app = builder.Build()

    app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore

    app.MapGet(
        "/recipes",
        Func<UseCase.ListRecipes, Task<RecipeListItem[]>>(fun listRecipes ->
            task {
                let! recipes = listRecipes ()
                return recipes |> List.toArray
            })
    )
    |> ignore

    app.Run()

    0 // Exit code