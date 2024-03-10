module HouseholdManager.AppHost.Adapters.PostgreSQL

open System
open System.Data
open Dapper
open Microsoft.Extensions.DependencyInjection

let configureServices connString (services: IServiceCollection) =
    DefaultTypeMap.MatchNamesWithUnderscores <- true

    services.AddScoped<IDbConnection>(
        Func<IServiceProvider, IDbConnection>(fun sc ->
            new Npgsql.NpgsqlConnection(connString))
    )