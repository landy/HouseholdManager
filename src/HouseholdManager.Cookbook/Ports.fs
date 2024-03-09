namespace HouseholdManager.Cookbook.Ports

open System.Threading.Tasks

type RecipeListItem = { Title: string; Image: string }
type RecipesListLoader = unit -> Task<RecipeListItem list>

[<RequireQualifiedAccess>]
module UseCase =
    type ListRecipes = unit -> Task<RecipeListItem list>