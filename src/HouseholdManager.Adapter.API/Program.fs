module HouseholdManager.Adapter.API.Host

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

let run (args: string[]) (setupServices: IServiceCollection -> unit) =
    let builder = WebApplication.CreateBuilder(args)
    setupServices builder.Services
    let app = builder.Build()

    app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore

    app.Run()

    0 // Exit code