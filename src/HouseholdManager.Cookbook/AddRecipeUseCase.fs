namespace HouseholdManager.Cookbook

open System
open System.Threading.Tasks
open FsToolkit.ErrorHandling


type AddRecipeRequest = { Title: string }
type ValidatedAddRecipeRequest = { Title: string }

type RecipeCreated =
    { Id: Guid
      Title: string
      Description: string }

type AddRecipeValidator =
    AddRecipeRequest -> Result<ValidatedAddRecipeRequest, string>

type RecipeSaver = RecipeCreated -> Task<unit>

type NewRecipeId = Guid

type AddRecipeWorkflow =
    NewRecipeId -> AddRecipeRequest -> Task<Result<Guid, string>>

[<RequireQualifiedAccess>]
module AddRecipeUseCase =
    let validateRequest (recipe: AddRecipeRequest) =
        if recipe.Title.Length > 0 then
            Ok { Title = recipe.Title }
        else
            Error "Title is required"

    let private toDomain newId (recipe: ValidatedAddRecipeRequest) =
        { Id = newId
          Title = recipe.Title
          Description = "" }


    let execute
        (validate: AddRecipeValidator)
        (saveRecipe: RecipeSaver)
        : AddRecipeWorkflow =
        fun (newRecipeId: NewRecipeId) (recipe: AddRecipeRequest) ->
            let saveRecipe = saveRecipe >> Task.map Ok

            validate recipe
            |> Result.map (toDomain newRecipeId)
            |> TaskResult.FromResult
            |> TaskResult.bind saveRecipe
            |> TaskResult.map (fun _ -> newRecipeId)