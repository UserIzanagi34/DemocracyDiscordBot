#!/bin/bash
git pull origin master
git submodule update --init --recursive
dotnet build DiscordDemocracyBot.sln --configuration Release -o ./bin/live_release
screen -dmS DiscordDemocracyBot dotnet bin/live_release/DiscordDemocracyBot.dll -- $1
