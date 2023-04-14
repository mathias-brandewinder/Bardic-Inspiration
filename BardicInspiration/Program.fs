namespace Archipendulum.BardicInspiration

module Program =

    open System.Threading.Tasks
    open DSharpPlus
    open DSharpPlus.CommandsNext
    open DSharpPlus.Net
    open DSharpPlus.Lavalink
    open DSharpPlus.AsyncEvents
    open Configuration

    [<EntryPoint>]
    let main argv =
        printfn "Starting."

        let token = appConfig.Token
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

        printfn "Connecting to Discord."
        discord.ConnectAsync()
        |> Async.AwaitTask
        |> Async.RunSynchronously

        printfn "Connecting to Lavalink."

        let lavalink = discord.UseLavalink()

        let lavalinkConfig =
            let config = appConfig.Lavalink
            let lavalinkEndpoint = ConnectionEndpoint(config.Hostname, config.Port)
            let lavalinkConfig = LavalinkConfiguration ()
            lavalinkConfig.Password <- config.Password
            lavalinkConfig.RestEndpoint <- lavalinkEndpoint
            lavalinkConfig.SocketEndpoint <- lavalinkEndpoint
            lavalinkConfig

        let lavalinkConnection =
            lavalink.ConnectAsync lavalinkConfig
            |> Async.AwaitTask
            |> Async.RunSynchronously

        printfn "Ready."

        Task.Delay(-1)
        |> Async.AwaitTask
        |> Async.RunSynchronously

        1