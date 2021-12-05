# Bardic Inspiration

## What is it

A Discord bot to help the Dungeon Master who wants to easily start "atmosphere" tracks on a loop, and switch between tracks.

The bot supports a few commands:

`/join`: join the General voice channel.

`/leave`: leave the current voice channel.

`/play https://youtu.be/dQw4w9WgXcQ`: start playing the track, and keep it on repeat. Starting a new track before another one completes will stop the track currently playing, and immediately start the new one.

`/stop`: stop the track that is currently playing.

`/pause`: pause the track currently playing.

`/resume`: resume playing the track currently paused.

## How to use it

To use Bardic Inspiration, you will need to run it locally, under your own account.

### Prerequisites

- Download [Lavalink](https://dsharpplus.github.io/articles/audio/lavalink/setup.html)
- Create your own Discord Bot in the [Discord Developer Portal](https://discord.com/developers/applications), to obtain your own token. Add your Discord Bot to the server(s) where you want to use it. See [this post](https://brandewinder.com/2021/10/30/fsharp-discord-bot/) for information.

### Running the Bot

- Start Lavalink.
- Build `BardicInspiration`.
- In `AppSettings.json`, modify the token to use your own.
- Run `BardicInspiration`.
- Start using the commands from your server!

## Guarantees and Terms of Service

None whatsoever.
