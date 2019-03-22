using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class PM : ModuleBase<SocketCommandContext>
    {
        [Command("PM"), RequireUserPermission(ChannelPermission.ManageChannels)]
        [Alias("pm", "Pm", "pM")]
        public async Task PmAsync(string user, [Remainder] string args)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            if (!ulong.TryParse(user, out var recipient))
            {
                foreach (var usr in Context.Guild.Users)
                {
                    if (usr.Username == user)
                    {
                        recipient = usr.Id;
                        
                    }
                }
                //throw new System.ArgumentException($"User \"{user}\" not found in current server"); 
            }
            var userObj = Context.Client.GetUser(recipient);
            await userObj.SendMessageAsync(args);
            Program.SendMessageBotChannel($"Send Private message to user: {userObj.Username}", 
                "Direct Message",
                Context.User.Username);
        }
    }
}