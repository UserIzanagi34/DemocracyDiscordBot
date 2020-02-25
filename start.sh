#!/bin/bash
git pull origin master
git submodule update --init --recursive
screen -dmS DiscordDemocracyBot dotnet run -- $1
