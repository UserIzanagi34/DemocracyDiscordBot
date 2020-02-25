using System;
using System.Collections.Generic;
using System.Text;
using DiscordBotBase;
using DiscordBotBase.CommandHandlers;
using Discord;
using Discord.WebSocket;
using Discord.Rest;
using FreneticUtilities.FreneticExtensions;
using FreneticUtilities.FreneticDataSyntax;

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
            if (!DemocracyBot.IsAdmin(message.Author))
            {
                return;
            }
            if (cmds.Length < 4)
            {
                SendErrorMessageReply(message, "Invalid Input", "Input does not look like it can possibly be valid. Use `!help` for usage information.");
                return;
            }
            string topicId = cmds[0].Replace("`", "").Trim();
            if (DemocracyBot.VoteTopicsSection.HasRootKeyLowered(topicId))
            {
                SendErrorMessageReply(message, "Topic Already Present", "That voting topic ID already exists. Pick a new one!");
                return;
            }
            StringBuilder topicTitle = new StringBuilder(100);
            topicTitle.Append(cmds[1].Replace("`", ""));
            int argId;
            for (argId = 2; argId < cmds.Length; argId++)
            {
                string arg = cmds[argId].Replace("`", "");
                if (arg.Trim() == "|")
                {
                    break;
                }
                topicTitle.Append(" ").Append(cmds[argId]);
            }
            List<string> choices = new List<string>(cmds.Length);
            StringBuilder currentChoice = new StringBuilder(100);
            for (argId++; argId < cmds.Length; argId++)
            {
                string arg = cmds[argId].Replace("`", "");
                if (arg.Trim() == "|")
                {
                    choices.Add(currentChoice.ToString());
                    currentChoice.Clear();
                    continue;
                }
                currentChoice.Append(" ").Append(cmds[argId]);
            }
            choices.Add(currentChoice.ToString());
            for (int i = 0; i < choices.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(choices[i]))
                {
                    choices.RemoveAt(i--);
                }
            }
            if (choices.IsEmpty())
            {
                SendErrorMessageReply(message, "Invalid Input", "No choices found.");
                return;
            }
            if (choices.Count == 1)
            {
                SendErrorMessageReply(message, "Insufficient Democracy", "Only 1 choice detected. Need at least 2.");
                return;
            }
            FDSSection newTopicSection = new FDSSection();
            newTopicSection.SetRoot("Topic", topicTitle.ToString());
            FDSSection choiceSection = new FDSSection();
            for (int i = 0; i < choices.Count; i++)
            {
                choiceSection.Set((i + 1).ToString(), choices[i]);
            }
            RestUserMessage sentMessage = message.Channel.SendMessageAsync(embed: GetGenericPositiveMessageEmbed("Vote In Progress", "New Vote... Data inbound, please wait!")).Result;
            newTopicSection.SetRoot("channel_id", sentMessage.Channel.Id);
            newTopicSection.SetRoot("post_id", sentMessage.Id);
            newTopicSection.SetRoot("Choices", choiceSection);
            newTopicSection.SetRoot("user_results", new FDSSection());
            DemocracyBot.VoteTopicsSection.SetRoot(topicId, newTopicSection);
            DemocracyBot.Save();
            RefreshTopicData(topicId, newTopicSection);
        }

        /// <summary>
        /// Refreshes the topic data in the publicly visible counting post.
        /// </summary>
        public static void RefreshTopicData(string topicId, FDSSection topicSection)
        {
            try
            {
                string topicTitle = topicSection.GetString("Topic");
                ulong channelId = topicSection.GetUlong("channel_id").Value;
                ulong postId = topicSection.GetUlong("post_id").Value;
                Console.WriteLine($"Try refresh of {topicId} via channel {channelId} at post {postId}.");
                StringBuilder choicesText = new StringBuilder(100);
                FDSSection choicesSection = topicSection.GetSection("Choices");
                foreach (string choice in choicesSection.GetRootKeys())
                {
                    choicesText.Append($"**{choice}**: `{choicesSection.GetString(choice)}`\n");
                }
                Embed embed = GetGenericPositiveMessageEmbed($"Vote For Topic **{topicId}**: {topicTitle}", $"Choices:\n{choicesText}\nVotes cast thus far: {topicSection.GetSection("user_results").Data.Count}\n\nDM this bot `!help` to cast your vote!");
                IMessage message = (DiscordBotBaseHelper.CurrentBot.Client.GetChannel(channelId) as SocketTextChannel).GetMessageAsync(postId).Result;
                (message as RestUserMessage).ModifyAsync(m => m.Embed = embed).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Topic data refresh for {topicId} failed: {ex}");
            }
        }

        /// <summary>
        /// Admin command to end an existing vote.
        /// </summary>
        public void CMD_EndVote(string[] cmds, SocketMessage message)
        {
            if (!DemocracyBot.IsAdmin(message.Author))
            {
                return;
            }
            // TODO
        }
    }
}
