namespace Archipendulum.BardicInspiration

open System
open FSharp.Control.Tasks
open DSharpPlus
open DSharpPlus.CommandsNext
open DSharpPlus.CommandsNext.Attributes
open DSharpPlus.Entities
open DSharpPlus.Lavalink
open Emzi0767.Utilities

type BardBot () =

    inherit BaseCommandModule ()

    member this.OnTrackStarted =
        AsyncEventHandler<LavalinkGuildConnection, EventArgs.TrackStartEventArgs>(
            fun (conn: LavalinkGuildConnection) (args: EventArgs.TrackStartEventArgs) ->
                unitTask {
                    printfn $"Starting track {args.Track.Title}."
                    }
            )

    member this.OnTrackFinished =
        AsyncEventHandler<LavalinkGuildConnection, EventArgs.TrackFinishEventArgs>(
            fun (conn: LavalinkGuildConnection) (args: EventArgs.TrackFinishEventArgs) ->
                unitTask {
                    printfn $"Finished track {args.Track.Title} ({args.Reason})."
                    match args.Reason with
                    | EventArgs.TrackEndReason.Finished ->
                        printfn $"Looping: restarting track {args.Track.Title}."
                        do! args.Player.PlayAsync(args.Track)
                    | _ -> ignore ()
                    }
            )

    [<Command "inspire">]
    [<Description "Cast bardic inspiration on someone!">]
    member this.Inspiration (ctx: CommandContext, [<Description "Who do you want to inspire?">] user: DiscordMember) =
        unitTask {
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

    [<Command "join">]
    [<Description "Join the General voice channel">]
    member this.Join (ctx: CommandContext) =
        unitTask {
            // find General voice channel
            let channelID, channel =
                ctx.Guild.Channels
                |> Seq.find (fun kv ->
                    kv.Value.Type = ChannelType.Voice
                    &&
                    kv.Value.Name.ToLowerInvariant () = "general"
                    )
                |> fun kv -> kv.Key, kv.Value

            let lavalink = ctx.Client.GetLavalink ()
            let node = lavalink.ConnectedNodes.Values |> Seq.head

            let! connection = node.ConnectAsync(channel)

            connection.add_PlaybackFinished(this.OnTrackFinished)
            connection.add_PlaybackStarted(this.OnTrackStarted)
            }

    [<Command "leave">]
    [<Description "Leave the current voice channel">]
    member this.Leave (ctx: CommandContext) =
        unitTask {
            let channel = ctx.Channel
            let lavalink = ctx.Client.GetLavalink ()
            let node = lavalink.ConnectedNodes.Values |> Seq.head
            let connection = node.GetGuildConnection(channel.Guild)
            connection.remove_PlaybackFinished(this.OnTrackFinished)
            connection.remove_PlaybackStarted(this.OnTrackStarted)
            do! connection.DisconnectAsync ()
            }

    [<Command "play">]
    [<Description "Search and play the requested track">]
    member this.Play (ctx: CommandContext, [<RemainingText>] search: string) =
        unitTask {
            let lavalink = ctx.Client.GetLavalink ()
            let node =
                lavalink.ConnectedNodes
                |> Seq.find (fun node ->
                    node.Value.ConnectedGuilds.ContainsKey(ctx.Guild.Id)
                    )
                |> fun kv -> kv.Value
            let connection = node.GetGuildConnection(ctx.Guild)

            let! loadResult = node.Rest.GetTracksAsync(search)

            let track = loadResult.Tracks |> Seq.head

            do! connection.PlayAsync(track)
            }

    [<Command "stop">]
    [<Description "Stop playing the current track">]
    member this.Stop (ctx: CommandContext) =
        unitTask {
            let lavalink = ctx.Client.GetLavalink ()
            let node =
                lavalink.ConnectedNodes
                |> Seq.find (fun node ->
                    node.Value.ConnectedGuilds.ContainsKey(ctx.Guild.Id)
                    )
                |> fun kv -> kv.Value
            let connection = node.GetGuildConnection(ctx.Guild)

            do! connection.StopAsync()
            }