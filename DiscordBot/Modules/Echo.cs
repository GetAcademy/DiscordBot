using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace MyBot.Modules
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