using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DiscordBotBase.CommandHandlers;
using Discord;
using Discord.WebSocket;
using FreneticUtilities.FreneticDataSyntax;
using FreneticUtilities.FreneticExtensions;

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
        public static bool IsUserAllowed(SocketMessage message)
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
                List<string> choices = topicSection.GetStringList($"user_results.{message.Author.Id}");
                if (choices != null)
                {
                    embed.AddField("You've already voted for:", $"`{string.Join(", ", choices)}`");
                }
                SendReply(message, embed.Build());
            }
        }

        /// <summary>
        /// Gets the topic section for a voting command. Also performs other basic prechecks.
        /// </summary>
        public static FDSSection GetVoteTopicSection(string[] cmds, SocketMessage message, out string topic)
        {
            topic = null;
            if (!IsUserAllowed(message))
            {
                return null;
            }
            if (DemocracyBot.VoteTopicsSection.IsEmpty())
            {
                SendGenericNegativeMessageReply(message, "No Current Voting", "Nothing to vote on. Check back later!");
                return null;
            }
            if (DemocracyBot.VoteTopicsSection.Data.Count == 1)
            {
                topic = DemocracyBot.VoteTopicsSection.Data.Keys.First();
            }
            else if (cmds.Length > 0)
            {
                topic = cmds[0].Replace("`", "").Replace(",", "").Replace(":", "").Trim();
            }
            if (topic == null)
            {
                SendErrorMessageReply(message, "Must Specify Topic", "There are currently multiple topics being voted on. Please specify which. Use `!ballot` to see the list of current topics!");
                return null;
            }
            FDSSection topicSection = DemocracyBot.VoteTopicsSection.GetSectionLowered(topic);
            if (topicSection == null)
            {
                SendErrorMessageReply(message, "Invalid Topic, Or Must Specify", "There are currently multiple topics being voted on. Please specify which. Use `!ballot` to see the list of current topics!"
                    + "\n\nIf you did specify a topic, you likely typed it in wrong. Remember, just use the short topic prefix, not the full name.");
                return null;
            }
            return topicSection;
        }

        /// <summary>
        /// User command to cast a vote.
        /// </summary>
        public void CMD_Vote(string[] cmds, SocketMessage message)
        {
            FDSSection topicSection = GetVoteTopicSection(cmds, message, out string topicName);
            if (topicSection == null)
            {
                return;
            }
            topicName = topicName.ToLowerFast();
            FDSSection choicesSection = topicSection.GetSection("Choices");
            List<string> newChoices = new List<string>();
            for (int i = 0; i < cmds.Length; i++)
            {
                string arg = cmds[i].Replace(",", "").Replace("`", "").Trim().ToLowerFast();
                if (arg == topicName && newChoices.IsEmpty())
                {
                    continue;
                }
                if (arg.ToLowerFast() == "none" && newChoices.IsEmpty())
                {
                    newChoices.Add("none");
                    break;
                }
                if (string.IsNullOrWhiteSpace(arg))
                {
                    continue;
                }
                if (choicesSection.GetRootDataLowered(arg) == null)
                {
                    SendErrorMessageReply(message, "Invalid Choice", $"Choice `{arg}` is not recognized. Did you format the command correctly?");
                    return;
                }
                if (newChoices.Contains(arg))
                {
                    SendErrorMessageReply(message, "Duplicate Choice", $"Choice `{arg}` has been sent twice. Check over your vote, you may have made a typo.");
                    return;
                }
                newChoices.Add(arg);
            }
            if (newChoices.IsEmpty())
            {
                SendErrorMessageReply(message, "Need To Choose", "You issued a vote command without any choices. You need to choose! If you're confused how to vote, use `!help`.");
                return;
            }
            List<string> originalChoices = topicSection.GetStringList($"user_results.{message.Author.Id}");
            topicSection.Set($"user_results.{message.Author.Id}", newChoices);
            DemocracyBot.Save();
            if (originalChoices == null)
            {
                SendGenericPositiveMessageReply(message, "Vote Cast", $"Your vote for topic `{topicName}` has been cast as: `{string.Join(", ", newChoices)}`.");
            }
            else
            {
                SendGenericPositiveMessageReply(message, "Vote Cast", $"Your vote for topic `{topicName}` has been replaced to: `{string.Join(", ", newChoices)}`.\n\nFor your own reference, here is your original vote for that topic: `{string.Join(", ", originalChoices)}`.");
            }
            AdminCommands.RefreshTopicData(topicName, topicSection, false);
        }

        /// <summary>
        /// User command to clear their vote.
        /// </summary>
        public void CMD_ClearVote(string[] cmds, SocketMessage message)
        {
            FDSSection topicSection = GetVoteTopicSection(cmds, message, out string topicName);
            if (topicSection == null)
            {
                return;
            }
            List<string> choices = topicSection.GetStringList($"user_results.{message.Author.Id}");
            if (choices == null)
            {
                SendGenericNegativeMessageReply(message, "Nothing To Clear", "You already do not have any vote cast to that voting topic.");
                return;
            }
            topicSection.Remove($"user_results.{message.Author.Id}");
            DemocracyBot.Save();
            SendGenericPositiveMessageReply(message, "Cleared", $"Your vote for topic `{topicName}` has been removed.\n\nFor your own reference, here is your original vote for that topic: `{string.Join(", ", choices)}`.");
        }
    }
}
