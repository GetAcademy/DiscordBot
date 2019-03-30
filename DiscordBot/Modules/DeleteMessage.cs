using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class DeleteMessage : ModuleBase<SocketCommandContext>
    {
        [Command("DeleteMessage"), Alias("deletemessage", "DELETEMESSAGE", "deleteMessage", "Deletemessage"), RequireUserPermission(GuildPermission.Administrator)]
        public async Task DeleteMessageAsync(string id, [Remainder] string args = "")
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);

            if (args.Contains("ALL"))
            {
                ulong.TryParse(id, out var userId);
                //var messages = new List<ulong>();
                await ReplyAsync("This function is not yet implemented");
            }
            else
            {
                ulong.TryParse(id, out var msgId);
                await Context.Channel.DeleteMessageAsync(msgId);
            }
        }
    }
}