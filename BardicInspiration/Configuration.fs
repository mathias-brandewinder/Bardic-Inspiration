namespace Archipendulum.BardicInspiration

module Configuration =

    open System
    open System.IO
    open Microsoft.Extensions.Configuration

    [<CLIMutable>]
    type Config = {
        MusicFolder: string
        }

    let token =
        "BARDICINSPIRATION_TOKEN"
        |> Environment.GetEnvironmentVariable

    let appConfig =
        ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json", true, true)
            .Build()
            .Get<Config>()