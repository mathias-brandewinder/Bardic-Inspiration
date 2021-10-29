namespace Archipendulum.BardicInspiration

open System
open FSharp.Control.Tasks
open DSharpPlus.CommandsNext
open DSharpPlus.CommandsNext.Attributes
open DSharpPlus.Entities

type BardBot () =

    inherit BaseCommandModule ()

    [<Command>]
    let inspiration (ctx: CommandContext) =
        unitTask {
            do!
                ctx.TriggerTypingAsync()

            let rng = Random ()
            let emoji =
                DiscordEmoji.FromName(ctx.Client, ":game_die:").ToString()

            rng.Next(1, 7)
            |> sprintf "%s Bardic Inspiration! Add %i to your next ability check, attack, or saving throw." emoji
            |> ctx.RespondAsync
            |> ignore
            }
