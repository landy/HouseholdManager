module HouseholdManager.Adapte.APITests.Cookbook

open System
open System.Threading.Tasks
open System.Data
open System.Net.Http.Json
open System.Text.Json
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection.Extensions
open Microsoft.Extensions.DependencyInjection
open NUnit.Framework
open FsUnit

open HouseholdManager.Cookbook


type CookbookApiFactory() =
    inherit WebApplicationFactory<HouseholdManager.AppHost.App.Program>()

    override self.ConfigureWebHost(webHostBuilder: IWebHostBuilder) =
        webHostBuilder.ConfigureTestServices(fun services ->
            services.RemoveAll(typeof<IDbConnection>) |> ignore
            services.RemoveAll(typeof<ListRecipesWorkflow>) |> ignore
            services.RemoveAll(typeof<AddRecipeWorkflow>) |> ignore
            let wf: ListRecipesWorkflow = fun _ -> Task.FromResult []

            services.AddScoped<ListRecipesWorkflow>(
                Func<IServiceProvider, ListRecipesWorkflow>(fun sp -> wf)
            )
            |> ignore

            let addRecipeWF: AddRecipeWorkflow =
                fun _ _ -> Task.FromResult(Result.Ok Guid.Empty)

            services.AddScoped<AddRecipeWorkflow>(
                Func<IServiceProvider, AddRecipeWorkflow>(fun sp ->
                    addRecipeWF)
            )
            |> ignore)
        |> ignore

[<Test>]
let ``List recipes should return ok`` () =
    task {
        use app = new CookbookApiFactory()
        let client = app.CreateClient()

        let! response = client.GetAsync("/recipes")

        response.StatusCode |> should equal System.Net.HttpStatusCode.OK
    }

[<Test>]
let ``Add recipe endpoint should return 400 when recipe is invalid`` () =
    task {
        use app = new CookbookApiFactory()

        let client =
            app
                .WithWebHostBuilder(fun webHostBuilder ->
                    webHostBuilder.ConfigureServices(fun services ->

                        services.RemoveAll(typeof<AddRecipeWorkflow>)
                        |> ignore

                        let addRecipeWF: AddRecipeWorkflow =
                            fun _ _ -> Task.FromResult(Result.Error "error")

                        services.AddScoped<AddRecipeWorkflow>(
                            Func<IServiceProvider, AddRecipeWorkflow>
                                (fun sp -> addRecipeWF)
                        )
                        |> ignore

                    )
                    |> ignore)
                .CreateClient()

        let addRecipe =
            { AddRecipeRequest.Title = "" } |> JsonSerializer.Serialize


        let! response = client.PostAsJsonAsync("/recipes", addRecipe)

        response.StatusCode |> should equal System.Net.HttpStatusCode.BadRequest
    }