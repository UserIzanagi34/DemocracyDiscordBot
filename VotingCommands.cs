using System;
using System.Collections.Generic;
using System.Text;
using DiscordBotBase.CommandHandlers;
using Discord;
using Discord.WebSocket;

namespace DemocracyDiscordBot
{
    /// <summary>
    /// Handler for voting related commands.
    /// </summary>
    public class VotingCommands : UserCommands
    {
        /// <summary>
        /// User command to see the available voting topics.
        /// </summary>
        public void CMD_Ballot(string[] cmds, SocketMessage message)
        {
            // TODO
        }

        /// <summary>
        /// User command to cast a vote.
        /// </summary>
        public void CMD_Vote(string[] cmds, SocketMessage message)
        {
            // TODO
        }
    }
}
