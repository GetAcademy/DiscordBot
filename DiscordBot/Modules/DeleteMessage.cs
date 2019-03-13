using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail.Model;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class DeleteMessage : ModuleBase<SocketCommandContext>
    {
        [Command("DeleteMessage"), RequireUserPermission(ChannelPermission.ManageChannels)]
        public async Task DeleteMessageAsync(string id, [Remainder] string args = "")
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);

            if (args.Contains("-ALL"))
            {
                ulong.TryParse(id, out var userID);
                //var messages = new List<ulong>();
                await ReplyAsync("This function is not yet implemented");
            }
            else
            {
                ulong.TryParse(id, out var msgID);
                await Context.Channel.DeleteMessageAsync(msgID);
            }
        }
    }
}