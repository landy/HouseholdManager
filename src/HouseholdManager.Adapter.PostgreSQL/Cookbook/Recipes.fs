module HouseholdManager.Adapter.PostgreSQL.Cookbook.Recipes

open System
open System.Data
open Dapper

open HouseholdManager.Cookbook

type private RecipeRow =
    { Id: Guid
      Title: string
      Description: string
      CreatedAt: DateTime
      UpdatedAt: DateTime }

let loadRecipes (conn: IDbConnection) : RecipesListLoader =
    fun () ->
        task {
            let query = "SELECT * FROM recipes"
            let! rows = conn.QueryAsync<RecipeRow>(query)

            return
                rows
                |> Seq.toList
                |> List.map (fun row -> { Title = row.Title; Image = "" })
        }

let saveCreatedRecipe (conn: IDbConnection) : RecipeSaver =
    fun (recipe: RecipeCreated) ->
        task {
            let query =
                "INSERT INTO recipes (id, title, description) VALUES (@Id, @Title, '')"

            let row =
                {| Id = recipe.Id
                   Title = recipe.Title |}

            let! _ = conn.ExecuteAsync(query, row)

            return ()
        }