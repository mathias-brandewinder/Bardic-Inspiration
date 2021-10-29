namespace Archipendulum.BardicInspiration

open System
open System.Threading.Tasks
open DSharpPlus.CommandsNext
open DSharpPlus.CommandsNext.Attributes
open DSharpPlus.Entities

type BardBot () =

    inherit BaseCommandModule ()

    [<Command>]
    let inspiration (ctx: CommandContext) =
        async {
            do!
                ctx.TriggerTypingAsync()
                |> Async.AwaitTask

            let rng = Random ()
            let emoji =
                DiscordEmoji.FromName(ctx.Client, ":game_die:").ToString()

            do!
                rng.Next(1, 7)
                |> sprintf "%s Bardic Inspiration! Add %i to your next ability check, attack, or saving throw." emoji
                |> ctx.RespondAsync
                |> Async.AwaitTask
                |> Async.Ignore
            }
        |> Async.StartAsTask
        :> Task