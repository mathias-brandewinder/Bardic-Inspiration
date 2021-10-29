namespace Archipendulum.BardicInspiration

module Program =

    open System.IO
    open Microsoft.Extensions.Configuration

    let config =
        ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json", true, true)
            .Build()

    [<EntryPoint>]
    let main argv =
        printfn "Starting"
        let token = config.["Token"]
        printfn "%s" token
        1