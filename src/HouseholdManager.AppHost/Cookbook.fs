module HouseholdManager.AppHost.Cookbook

open System
open System.Data
open Microsoft.Extensions.DependencyInjection

open HouseholdManager.Cookbook
open HouseholdManager.Adapter.PostgreSQL

let private configureListRecipes (services: IServiceCollection) =
    services.AddScoped<ListRecipesWorkflow>(
        Func<IServiceProvider, ListRecipesWorkflow>(fun sc ->
            let conn = sc.GetRequiredService<IDbConnection>()
            let loader = Cookbook.Recipes.loadRecipes conn
            ListRecipesUseCase.execute loader)
    )

let configureAddRecipe (services: IServiceCollection) =
    services.AddScoped<AddRecipeWorkflow>(
        Func<IServiceProvider, AddRecipeWorkflow>(fun sc ->
            let conn = sc.GetRequiredService<IDbConnection>()
            let adder = Cookbook.Recipes.saveCreatedRecipe conn
            let validator = AddRecipeUseCase.validateRequest
            AddRecipeUseCase.execute validator adder)
    )

let configureServices (services: IServiceCollection) =
    configureListRecipes services |> configureAddRecipe