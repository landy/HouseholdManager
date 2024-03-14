module HouseholdManager.Adapter.API.Host

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting


type HouseholdManagerApi() =
    member _.Run
        (args: string[])
        (setupServices: ConfigurationManager -> IServiceCollection -> unit)
        =
        let builder = WebApplication.CreateBuilder(args)

        setupServices builder.Configuration builder.Services

        let app = builder.Build()

        app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore

        Cookbook.registerEndpoints app

        app.Run()

        0 // Exit code