using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Discord.Net;
using Discord;
using Discord.WebSocket;
using System.Diagnostics;
using FreneticUtilities.FreneticExtensions;
using FreneticUtilities.FreneticDataSyntax;
using DiscordBotBase;
using DiscordBotBase.CommandHandlers;
using DemocracyDiscordBot.CommandHandlers;

namespace DemocracyDiscordBot
{
    /// <summary>
    /// General program entry and handler.
    /// </summary>
    public class DemocracyBot
    {
        /// <summary>
        /// The ID of the guild this bot is for.
        /// </summary>
        public static ulong OwningGuildID;

        /// <summary>
        /// The FDS Section of current vote topics.
        /// </summary>
        public static FDSSection VoteTopicsSection;

        public static HashSet<ulong> Admins;

        /// <summary>
        /// Save the config, after any votes are cast.
        /// </summary>
        public static void Save()
        {
            DiscordBotBaseHelper.CurrentBot.SaveConfig();
        }

        /// <summary>
        /// Helper method to determine if a user is a bot admin.
        /// </summary>
        public static bool IsAdmin(SocketUser user)
        {
            return Admins.Contains(user.Id);
        }

        /// <summary>
        /// Software entry point - starts the bot.
        /// </summary>
        static void Main(string[] args)
        {
            DiscordBotBaseHelper.StartBotHandler(args, new DiscordBotConfig()
            {
                CommandPrefix = "!",
                Initialize = (bot) =>
                {
                    InfoCommands infoCommands = new InfoCommands() { Bot = bot };
                    VotingCommands voteCommands = new VotingCommands() { Bot = bot };
                    CoreCommands coreCommands = new CoreCommands(IsAdmin) { Bot = bot };
                    AdminCommands adminCommands = new AdminCommands() { Bot = bot };
                    OwningGuildID = bot.ConfigFile.GetUlong("guild_id").Value;
                    VoteTopicsSection = bot.ConfigFile.GetSection("vote_topics");
                    if (VoteTopicsSection == null)
                    {
                        VoteTopicsSection = new FDSSection();
                        bot.ConfigFile.Set("vote_topics", VoteTopicsSection);
                    }
                    Admins = new HashSet<ulong>(bot.ConfigFile.GetStringList("admins").Select(s => ulong.Parse(s)));
                    // Info
                    bot.RegisterCommand(infoCommands.CMD_Help, "help", "halp", "hlp", "hal", "hel", "h", "?", "use", "usage");
                    bot.RegisterCommand(infoCommands.CMD_Hello, "hello", "hi", "whoareyou", "what", "link", "info");
                    // Voting
                    bot.RegisterCommand(voteCommands.CMD_Ballot, "ballot", "b");
                    bot.RegisterCommand(voteCommands.CMD_Vote, "vote", "v");
                    bot.RegisterCommand(voteCommands.CMD_ClearVote, "clearvote", "voteclear", "cv");
                    // Admin
                    bot.RegisterCommand(coreCommands.CMD_Restart, "restart");
                    bot.RegisterCommand(adminCommands.CMD_CallVote, "callvote");
                    bot.RegisterCommand(adminCommands.CMD_EndVote, "endvote");
                    bot.Client.Ready += () =>
                    {
                        bot.Client.SetGameAsync("With the balance of power in this world.").Wait();
                        return Task.CompletedTask;
                    };
                },
                ShouldPayAttentionToMessage = (message) =>
                {
                    return message.Channel is IPrivateChannel || IsAdmin(message.Author);
                }
            });
        }
    }
}
