using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MyBot.Modules
{
    public class Invite : ModuleBase<SocketCommandContext>
    {

        [Command("Invite"), RequireUserPermission(ChannelPermission.ManageChannels)]
        [Alias("INVITE", "invite", "inv", "INV")]

        public async Task InviteUserAsync([Remainder] string args)
        {
            var arguments = args.Split(' ');
            //var inv = Context.Client.GetInviteAsync("540248332069765128");

            ulong.TryParse(arguments[0], out var recipient);
            if (recipient.ToString().Length > 1)
            {
                var user = Context.Client.GetUser(recipient);
                await user.SendMessageAsync("PLACEHOLDER");
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = "It's Dangerous to go alone!",
                    Description = "FUCKWITS!"
                };
                await ReplyAsync("", false, embed.Build());
            }
        }
}
}