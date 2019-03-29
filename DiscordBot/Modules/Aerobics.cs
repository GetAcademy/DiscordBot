using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class Aerobics : ModuleBase<SocketCommandContext>
    {
        [Command("Aerobics"), RequireUserPermission(GuildPermission.ManageChannels)]
        [Alias("")]
        [Summary("")]
        public async Task AerobicsAsync([Remainder] string remainder)
        {
            ReplyAsync("https://www.youtube.com/watch?v=TIfAkOBMf5A");
        }
    }
}