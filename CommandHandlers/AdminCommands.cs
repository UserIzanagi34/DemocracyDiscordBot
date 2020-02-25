using System;
using System.Collections.Generic;
using System.Text;
using DiscordBotBase.CommandHandlers;
using Discord;
using Discord.WebSocket;

namespace DemocracyDiscordBot.CommandHandlers
{
    /// <summary>
    /// Admin commands handler.
    /// </summary>
    public class AdminCommands : UserCommands
    {
        /// <summary>
        /// Admin command to start a new vote.
        /// </summary>
        public void CMD_CallVote(string[] cmds, SocketMessage message)
        {
            // TODO
        }

        /// <summary>
        /// Admin command to end an existing vote.
        /// </summary>
        public void CMD_EndVote(string[] cmds, SocketMessage message)
        {
            // TODO
        }
    }
}
