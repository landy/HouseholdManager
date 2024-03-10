module HouseholdManager.CookbookTests

open System
open System.Collections.Concurrent
open NUnit.Framework
open FsUnit

open HouseholdManager.Cookbook
open HouseholdManager.Cookbook.Domain

[<TestCase("", true)>]
[<TestCase("Test", false)>]
let ``Recipe title validator returns a correct result`` (title, expected) =
    let recipe: AddRecipeRequest = { Title = title }

    let result = AddRecipeUseCase.validateRequest recipe

    result |> Result.isError |> should equal expected

[<Test>]
let ``Adding a valid recipe to the cookbook should succeed`` () =
    task {
        let recipe: AddRecipeRequest = { Title = "test" }
        let validator: AddRecipeValidator = fun _ -> Ok { Title = "" }
        let saver: RecipeSaver = fun _ -> task { return () }

        let! expected =
            AddRecipeUseCase.execute validator saver Guid.Empty recipe

        expected |> Result.isOk |> should equal true
    }

[<Test>]
let ``Adding a valid recipe stores the recipe`` () =
    task {
        let recipeStore = ConcurrentDictionary<Guid, Recipe>()
        let recipe: AddRecipeRequest = { Title = "test" }
        let validator: AddRecipeValidator = fun _ -> Ok { Title = "" }

        let saver: RecipeSaver =
            fun r -> task { recipeStore.TryAdd(Guid.Empty, r) |> ignore }

        let! _ = AddRecipeUseCase.execute validator saver Guid.Empty recipe

        recipeStore.Count |> should equal 1
    }