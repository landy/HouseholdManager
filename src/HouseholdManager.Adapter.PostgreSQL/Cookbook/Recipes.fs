module HouseholdManager.Adapter.PostgreSQL.Cookbook.Recipes

open System
open System.Data
open Dapper

open HouseholdManager.Cookbook.Ports

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