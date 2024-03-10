module HouseholdManager.Adapter.API.Cookbook

open System
open System.Threading.Tasks
open HouseholdManager.Cookbook
open Microsoft.AspNetCore.Builder

let registerEndpoints (app: WebApplication) =
    app.MapGet(
        "/recipes",
        Func<ListRecipesWorkflow, Task<RecipeListItem[]>>(fun listRecipes ->
            task {
                let! recipes = listRecipes ()
                return recipes |> List.toArray
            })
    )
    |> ignore

    app.MapPost(
        "/recipes",
        Func<AddRecipeWorkflow, AddRecipeRequest, Task<Guid>>
            (fun createRecipe recipe ->
                let newRecipeId = Guid.NewGuid()

                task {
                    let! _ = createRecipe newRecipeId recipe
                    return newRecipeId
                })
    )
    |> ignore