using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class AssignStudentRole : ModuleBase<SocketCommandContext>
    {
        [Command("AssignStudentRole"), RequireUserPermission(GuildPermission.ManageChannels)]
        [Alias("ASR")]
        [Summary("")]
        public async Task AssignStudentRoleAsync(string id)
        {
            ulong.TryParse(id, out ulong result);
            Console.Write(result);
            await DiscordBot.Program.AssignStudentRole(Context.Guild.GetUser(result));
        }
    }
}