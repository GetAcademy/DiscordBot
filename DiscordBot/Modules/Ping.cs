using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("Ping")]
        [Alias("PING")]
        public async Task PingAsync()
        {
            //await ReplyAsync("Hello World!");
            
            await ReplyAsync($"{Context.User.Mention} !PONG!");
        }
    }
}