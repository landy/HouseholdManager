namespace HouseholdManager.Cookbook

open System.Threading.Tasks


type RecipeListItem = { Title: string; Image: string }
type RecipesListLoader = unit -> Task<RecipeListItem list>
type ListRecipesWorkflow = unit -> Task<RecipeListItem list>

[<RequireQualifiedAccess>]
module ListRecipesUseCase =
    let execute (recipesGetter: RecipesListLoader) : ListRecipesWorkflow =
        fun () ->
            task {
                let! recipes = recipesGetter ()
                return recipes
            }