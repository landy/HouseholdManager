open HouseholdManager.Adapter.API
open Microsoft.Extensions.DependencyInjection

let private setupServices (services: IServiceCollection) = ()

[<EntryPoint>]
let main args =


    Host.run args setupServices