using System;
using System.Collections.Generic;
using System.Text;
using DiscordBotBase.CommandHandlers;
using Discord;
using Discord.WebSocket;

namespace DemocracyDiscordBot.CommandHandlers
{
    /// <summary>
    /// Informational user commands handler.
    /// </summary>
    public class InfoCommands : UserCommands
    {
        /// <summary>
        /// User command to get help (shows a list of valid bot commands).
        /// </summary>
        public void CMD_Help(string[] cmds, SocketMessage message)
        {
            EmbedBuilder embed = new EmbedBuilder().WithTitle("Bot Command Help").WithFooter("Powered by Democracy!");
            embed.AddField("**Available Informational Commands:**", "`!hello` for basic bot info, `!help` for usage info");
            embed.AddField("**Available Voting Commands:**", "`!ballot` to see what's current available to vote for, `!vote [topic] <choices ...>` to choose your votes, `!clearvote` to cancel your previous vote.");
            embed.AddField("**How To Vote:**", "1: Pull open the `!ballot`\n2: Pick the topic you'd like to cast your vote on (let's say for example, 'Topic A: Who's the new king?')"
                + "\n3: Pick what choices you like (let's say you like '1: Bob' and will accept '3: Joe', but don't want '2: Steve.'.\n4: Enter your choices to the `!vote` command "
                + "(for those examples, you would type `!vote A 1 3`. This would cast your vote for Topic A as preferring Bob most, and Joe as secondary, but not supporting Steve at all)."
                + "\nNote that you can input 'none' as your choices input to indicate you don't have any preference but want the total number of members who voted to be 1 higher.");
            SendReply(message, embed.Build());
        }

        /// <summary>
        /// User command to get basic bot info.
        /// </summary>
        public void CMD_Hello(string[] cmds, SocketMessage message)
        {
            SendGenericPositiveMessageReply(message, "Discord Democracy Bot", "Hello! I'm the Discord Democracy bot, written by Alex \"mcmonkey\" Goodwin.\nI'm 100% Free And Open Source on GitHub at <https://github.com/mcmonkeyprojects/DemocracyDiscordBot>!");
        }
    }
}
