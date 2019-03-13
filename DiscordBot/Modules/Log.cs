using System.Threading.Tasks;
using Amazon.CloudFront.Model;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class Log : ModuleBase<SocketCommandContext>
    {
        [Command("Log"), RequireUserPermission(ChannelPermission.ManageChannels)]
        [Alias("log", "LOG")]
        public async Task LogAsync()
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            MyBot.Program.SendMessageBotChannel($"Send logfile to: {Context.User.Username}", "Logfile report", Context.User.Username);
            await Context.User.SendFileAsync(
                "I:/GET/DiscordBot/GETSharp/CsharpGetBot/DiscordBot/DiscordBot/bin/Debug/logfile.txt", "");
        }
    }
}