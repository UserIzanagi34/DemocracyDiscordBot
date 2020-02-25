Democracy Discord Bot
---------------------

A Discord bot that implements a private ranked-choice voting system for Discord guilds to use.

## Setup

Gathering the files note:
- In addition to a `git clone` of this repository, you need to clone the sub-repository, using a command like `git submodule update --init --recursive`.

The `start.sh` file is used by the `restart` command and should be maintained as correct to the environment to launch a new bot program instance... points of note:
- It starts with a `git pull` command to self-update. If this is not wanted, remove it. Be careful what repository this will pull from (a fork you own vs. the original repository vs. some other one...)
- It uses a `screen` command to launch the bot quietly into a background screen. The `screen` program must be installed for that to work. Alternately, replace it with some other equivalent background terminal program.
- The restart command will run this script equivalently to the following terminal command: `bash ./start.sh 12345` where `12345` is the ID number for the channel that issued a restart command.

To start the bot up:
- Run `./start.sh` while in the bot's directory.

To view the bot's terminal:
- Connect to the screen - with an unaltered `start.sh` file, the way to connect to that is by running `screen -r DemocracyDiscordBot`.

## Copyright/Legal Info

The MIT License (MIT)

Copyright (c) 2020 Alex "mcmonkey" Goodwin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
