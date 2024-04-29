namespace Archipendulum.BardicInspiration

module Program =

    open System.Threading.Tasks
    open DSharpPlus
    open DSharpPlus.CommandsNext
    open DSharpPlus.VoiceNext

    [<EntryPoint>]
    let main argv =
        printfn "Starting."

        let token = Configuration.token
        let config = DiscordConfiguration ()
        config.Token <- token
        config.TokenType <- TokenType.Bot
        // TODO: figure out minimal set of intents needed
        // Note: use ||| to select multiple intents
        config.Intents <- DiscordIntents.All

        let discord = new DiscordClient(config)

        let commandsConfig = CommandsNextConfiguration ()
        commandsConfig.StringPrefixes <- ["/"]

        let commands = discord.UseCommandsNext(commandsConfig)
        commands.RegisterCommands<BardBot>()

        discord.UseVoiceNext() |> ignore

        printfn "Connecting to Discord."
        discord.ConnectAsync()
        |> Async.AwaitTask
        |> Async.RunSynchronously

        printfn "Ready."

        Task.Delay(-1)
        |> Async.AwaitTask
        |> Async.RunSynchronously

        1