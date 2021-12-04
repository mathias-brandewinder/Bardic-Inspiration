namespace Archipendulum.BardicInspiration

module Configuration =

    open System.IO
    open Microsoft.Extensions.Configuration

    [<CLIMutable>]
    type Lavalink = {
        Hostname: string
        Port: int
        Password: string
        }

    [<CLIMutable>]
    type Config = {
        Token: string
        Lavalink: Lavalink
        }

    let appConfig =
        ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json", true, true)
            .Build()
            .Get<Config>()