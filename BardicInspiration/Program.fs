namespace Archipendulum.BardicInspiration

module Program =

    open System.Threading.Tasks
    open System.IO
    open Microsoft.Extensions.Configuration
    open DSharpPlus
    open DSharpPlus.CommandsNext
    open DSharpPlus.Net
    open DSharpPlus.Lavalink

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

        let discord = new DiscordClient(config)

        let commandsConfig = CommandsNextConfiguration ()
        commandsConfig.StringPrefixes <- ["/"]

        let commands = discord.UseCommandsNext(commandsConfig)
        commands.RegisterCommands<BardBot>()

        printfn "Connecting to Discord"
        discord.ConnectAsync()
        |> Async.AwaitTask
        |> Async.RunSynchronously

        printfn "Connecting to Lavalink"
        let hostname = appConfig.["Lavalink:Hostname"]
        let port = appConfig.["Lavalink:Port"] |> int
        let password = appConfig.["Lavalink:Password"]

        let lavalinkEndpoint = ConnectionEndpoint(hostname, port)
        let lavalinkConfig = LavalinkConfiguration ()
        lavalinkConfig.Password <- password
        lavalinkConfig.RestEndpoint <- lavalinkEndpoint
        lavalinkConfig.SocketEndpoint <- lavalinkEndpoint

        let lavalink = discord.UseLavalink()

        let lavalinkConnection =
            lavalink.ConnectAsync lavalinkConfig
            |> Async.AwaitTask
            |> Async.RunSynchronously

        Task.Delay(-1)
        |> Async.AwaitTask
        |> Async.RunSynchronously

        1