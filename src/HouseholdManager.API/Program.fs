module HouseholdManager.API.Host
open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting

let main args =
    let builder = WebApplication.CreateBuilder(args)
    let app = builder.Build()

    app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore

    app.Run()

    0 // Exit code

