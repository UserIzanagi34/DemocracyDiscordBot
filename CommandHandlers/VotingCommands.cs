using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DiscordBotBase.CommandHandlers;
using Discord;
using Discord.WebSocket;
using FreneticUtilities.FreneticDataSyntax;

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
            return message.Author.MutualGuilds.Any(guild => guild.Id == DemocracyBot.OwningGuildID);
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
            if (DemocracyBot.VoteTopicsSection.IsEmpty())
            {
                SendGenericNegativeMessageReply(message, "No Current Voting", "Nothing to vote on. Check back later!");
                return;
            }
            foreach (string topic in DemocracyBot.VoteTopicsSection.GetRootKeys())
            {
                FDSSection topicSection = DemocracyBot.VoteTopicsSection.GetSection(topic);
                EmbedBuilder embed = new EmbedBuilder().WithTitle($"Topic **{topic}**: {topicSection.GetString("Topic")}");
                FDSSection choicesSection = topicSection.GetSection("Choices");
                foreach (string choice in choicesSection.GetRootKeys())
                {
                    embed.AddField($"Option **{choice}**:", choicesSection.GetString(choice));
                }
                SendReply(message, embed.Build());
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

        /// <summary>
        /// User command to clear their vote.
        /// </summary>
        public void CMD_ClearVote(string[] cmds, SocketMessage message)
        {
            if (!IsUserAllowed(message))
            {
                return;
            }
            // TODO
        }
    }
}
