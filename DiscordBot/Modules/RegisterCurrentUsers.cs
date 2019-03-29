using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class RegisterCurrentUsers : ModuleBase<SocketCommandContext>
    {
        [Command("RegisterCurrentUsers"), RequireUserPermission(GuildPermission.Administrator)]
        [Alias("RCU", "RegisterUsers")]
        public async Task RegisterCurrentUsersAsync()
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            //var userIds = new List<ulong>();
            var userNames = "";
            foreach (var user in Program.GetServer.Users)
            {
                if (user.VoiceChannel == null || user.VoiceChannel.Category.Id != Program.DefaultCategory) continue;
                //userIds.Add(user.Id);
                userNames += $"__{user.Username}__ nick: **{user.Nickname}**\n";
            }


            await ReplyAsync("Registered!");
            await Context.User.SendMessageAsync(userNames);
        }
    }
}