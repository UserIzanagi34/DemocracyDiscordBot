using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DiscordBotBase.CommandHandlers;
using Discord;
using Discord.WebSocket;

namespace DemocracyDiscordBot.CommandHandlers
{
    /// <summary>
    /// Handler for voting related commands.
    /// </summary>
    public class VotingCommands : UserCommands
    {
        /// <summary>
        /// Checks whether a user is allowed to interact with the bot.
        /// </summary>
        public bool IsUserAllowed(SocketMessage message)
        {
            return message.Author.MutualGuilds.Any(guild => guild.Id == Program.OwningGuildID);
        }

        /// <summary>
        /// User command to see the available voting topics.
        /// </summary>
        public void CMD_Ballot(string[] cmds, SocketMessage message)
        {
            if (!IsUserAllowed(message))
            {
                return;
            }
            // TODO
        }

        /// <summary>
        /// User command to cast a vote.
        /// </summary>
        public void CMD_Vote(string[] cmds, SocketMessage message)
        {
            if (!IsUserAllowed(message))
            {
                return;
            }
            // TODO
        }
    }
}
