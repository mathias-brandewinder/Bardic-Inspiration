namespace Archipendulum.BardicInspiration

open System
open System.IO
open System.Diagnostics
open System.Threading.Tasks

open DSharpPlus
open DSharpPlus.CommandsNext
open DSharpPlus.CommandsNext.Attributes
open DSharpPlus.Entities
open DSharpPlus.VoiceNext

open NAudio.Wave

type BardBot () =

    inherit BaseCommandModule ()

    let mutable tokenSource = new Threading.CancellationTokenSource()

    [<Command "inspire">]
    [<Description "Cast bardic inspiration on someone!">]
    member this.Inspiration (ctx: CommandContext, [<Description "Who do you want to inspire?">] user: DiscordMember) =
        task {
            do!
                ctx.TriggerTypingAsync()

            let emoji = DiscordEmoji.FromName(ctx.Client, ":drum:").Name
            let roll = Random().Next(1, 7)
            let userName = user.Mention

            let! _ =
                $"{emoji} Bardic Inspiration! {userName}, add {roll} (1d6) to your next ability check, attack, or saving throw."
                |> ctx.RespondAsync

            return ()
            }
            :> Task

    [<Command "join">]
    [<Description "Join the General voice channel">]
    member this.Join (ctx: CommandContext) =
        task {
            printfn "connecting"
            // find General voice channel
            let channelID, channel =
                ctx.Guild.Channels
                |> Seq.find (fun kv ->
                    kv.Value.Type = ChannelType.Voice
                    &&
                    kv.Value.Name.ToLowerInvariant () = "general"
                    )
                |> fun kv -> kv.Key, kv.Value
            let! _ = channel.ConnectAsync()
            printfn "connected"
            }
            :> Task

    [<Command "leave">]
    [<Description "Leave the current voice channel">]
    member this.Leave (ctx: CommandContext) =
        task {
            printfn "disconnecting"
            let voiceClient = ctx.Client.GetVoiceNext()
            let connection = voiceClient.GetConnection(ctx.Guild)
            connection.Disconnect()
            printfn "disconnected"
            }
            :> Task

    member private this.LoopTrack (file: FileInfo, transmitSink: VoiceTransmitSink) =
        task {
            printfn $"playing {file.Name}"

            tokenSource <- new Threading.CancellationTokenSource()
            let token = tokenSource.Token
            let buffer = Memory(Array.init 3840 (fun _ -> 0uy))

            let reader = new Mp3FileReader(file.FullName)

            let! firstRead = reader.ReadAsync(buffer, token)

            printfn $"first read {firstRead}"
            let mutable hasData = firstRead > 0

            while hasData do
                if token.IsCancellationRequested
                then
                    printfn $"Stopping {file.Name}"
                    do! reader.DisposeAsync()
                    do! transmitSink.FlushAsync()
                    tokenSource.Dispose()
                else
                    do! transmitSink.WriteAsync(buffer, token)
                    let! nextRead = reader.ReadAsync(buffer, token)
                    hasData <- (nextRead > 0)

            if (not hasData)
            then
                printfn $"End of track, looping {file.Name}"
                return! this.LoopTrack(file, transmitSink)
            else ignore ()
        }

    [<Command "play">]
    [<Description "Search and play the requested track">]
    member this.Play (ctx: CommandContext, [<RemainingText>] search: string) =
        task {
            printfn "play"

            let voiceClient = ctx.Client.GetVoiceNext()
            let connection = voiceClient.GetConnection(ctx.Guild)
            let transmitSink = connection.GetTransmitSink()

            if (not (isNull tokenSource))
            then
                printfn "stopping previous track"
                tokenSource.Cancel()
                do! transmitSink.FlushAsync()
                tokenSource.Dispose()

            let filePath =
                try
                    let folder =
                        Configuration.appConfig.MusicFolder
                        |> System.IO.DirectoryInfo

                    let tracks =
                        folder.EnumerateFiles()
                        |> Seq.filter (fun fileInfo ->
                            fileInfo.Extension.ToUpperInvariant() = ".MP3"
                            )
                        |> Seq.toArray

                    let index = search |> int
                    if index < tracks.Length
                    then tracks.[index].FullName |> Some
                    else None
                with
                | _ -> None

            match filePath with
            | None -> printfn "no file to play"
            | Some filePath ->

                do! this.LoopTrack(FileInfo filePath, transmitSink)
                printfn $"Stopped looping {filePath}"
            }
            :> Task

    [<Command "stop">]
    [<Description "Stop playing the current track">]
    member this.Stop (ctx: CommandContext) =
        task {
            printfn "stop"
            tokenSource.Cancel()
            }
        :> Task

    [<Command "pause">]
    [<Description "Pause playing the current track">]
    member this.Pause (ctx: CommandContext) =
        task {
            let voiceClient = ctx.Client.GetVoiceNext()
            let connection = voiceClient.GetConnection(ctx.Guild)
            connection.Pause()
            }
        :> Task

    [<Command "resume">]
    [<Description "Resume playing the current paused track">]
    member this.Resume (ctx: CommandContext) =
        task {
            let voiceClient = ctx.Client.GetVoiceNext()
            let connection = voiceClient.GetConnection(ctx.Guild)
            do! connection.ResumeAsync()
            }
        :> Task

    [<Command "tracks">]
    [<Description "Cast bardic inspiration on someone!">]
    member this.Tracks (ctx: CommandContext) =
        task {
            let folder =
                Configuration.appConfig.MusicFolder
                |> System.IO.DirectoryInfo

            let tracks =
                folder.EnumerateFiles()
                |> Seq.filter (fun fileInfo ->
                    fileInfo.Extension.ToUpperInvariant() = ".MP3"
                    )
                |> Seq.mapi (fun i file -> $"[{i}]: {file.Name}")
                |> String.concat (Environment.NewLine)

            printfn $"{tracks}"

            let! _ = ctx.RespondAsync(tracks)
            return ()
            }
            :> Task