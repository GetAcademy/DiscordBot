using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class PM : ModuleBase<SocketCommandContext>
    {
        [Command("PM"), RequireUserPermission(ChannelPermission.ManageChannels)]
        [Alias("pm", "Pm", "pM")]
        public async Task PmAsync(string user, [Remainder] string args)
        {
            ulong.TryParse(user, out var recipient);
            var userObj = Context.Client.GetUser(recipient);
            await userObj.SendMessageAsync(args);
            MyBot.Program.SendMessageBotChannel($"Send Private message to user: {userObj.Username}", 
                "Direct Message",
                Context.User.Username);
        }
    }
}