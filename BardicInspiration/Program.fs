namespace Archipendulum.BardicInspiration

module Program =

    open System.IO
    open Microsoft.Extensions.Configuration
    open DSharpPlus

    let appConfig =
        ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json", true, true)
            .Build()

    [<EntryPoint>]
    let main argv =
        printfn "Starting"

        let token = appConfig.["Token"]
        let config = DiscordConfiguration ()
        config.Token <- token
        config.TokenType <- TokenType.Bot

        let client = new DiscordClient(config)

        1