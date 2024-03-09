module HouseholdManager.Cookbook.ListRecipesUseCase

open HouseholdManager.Cookbook.Ports

let execute (recipesGetter: RecipesListLoader) =
    task {
        let! recipes = recipesGetter ()
        return recipes
    }