module HouseholdManager.Adapter.PostgreSQL.Cookbook.Recipes

open System
open System.Data
open Dapper

open HouseholdManager.Cookbook
open HouseholdManager.Cookbook.Domain

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

let addRecipe (conn: IDbConnection) : RecipeSaver =
    fun (recipe: Recipe) ->
        task {
            let query =
                "INSERT INTO recipes (id, title, description) VALUES (@Id, @Title, '')"

            let row =
                {| Id = recipe.Id
                   Title = recipe.Title |}

            let! _ = conn.ExecuteAsync(query, row)

            return ()
        }