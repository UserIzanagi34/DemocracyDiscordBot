#!/bin/bash
git pull origin master
git submodule update --init --recursive
dotnet build DemocracyDiscordBot.sln --configuration Release -o ./bin/live_release
screen -dmS DemocracyDiscordBot dotnet bin/live_release/DemocracyDiscordBot.dll -- $1
