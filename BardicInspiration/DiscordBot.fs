namespace Archipendulum.BardicInspiration

open System
open FSharp.Control.Tasks
open DSharpPlus.CommandsNext
open DSharpPlus.CommandsNext.Attributes
open DSharpPlus.Entities

type BardBot () =

    inherit BaseCommandModule ()

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