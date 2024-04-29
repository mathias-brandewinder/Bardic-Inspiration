# Bardic Inspiration

## What is it

A Discord bot to help the Dungeon Master who wants to easily start "atmosphere" 
tracks on a loop, and switch between tracks.

The bot supports a few commands:

`/join`: join the General voice channel.

`/leave`: leave the current voice channel.

`/tracks`: display available tracks.

`/play 1`: start playing track 1, and keep it on repeat. Starting a new track 
before another one completes will stop the track currently playing, and 
immediately start the new one.

`/stop`: stop the track that is currently playing.

`/pause`: pause the track currently playing.

`/resume`: resume playing the track currently paused.

## How to use it

To use Bardic Inspiration, you will need to run it locally, under your own account.

### Prerequisites

- Create your own Discord Bot in the [Discord Developer Portal](https://discord.com/developers/applications), to obtain your own token. 
- Create an environment variable `BARDICINSPIRATION_TOKEN` for your token.
- Add your Discord Bot to the server(s) where you want to use it. See [this post](https://brandewinder.com/2021/10/30/fsharp-discord-bot/) for information.
- In `AppSettings.json`, set `MusicFolder` to a folder containing `mp3` files.

### Running the Bot

- Build `BardicInspiration`.
- Create an environment variable `BARDICINSPIRATION_TOKEN` for your token.
- In `AppSettings.json`, modify `MusicFolder` to point a folder containing `mp3` files.
- Run `BardicInspiration`.
- Start using the commands from your server!

## Changes

### Version 2.0

Ditched Lavalink, which was not supported in DSharpPlus anymore. As a result, 
directly streaming music from YouTube is gone. The bot will now play your tracks, 
which must be available locally.

## Guarantees and Terms of Service

None whatsoever.
