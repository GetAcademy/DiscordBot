using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class Echo : ModuleBase<SocketCommandContext>
    {
        [Command("Echo")]
        [Alias("echo", "ECHO")]
        public async Task EchoAsync([Remainder] string reply)
        {
            await ReplyAsync(reply);
        }
    }
}