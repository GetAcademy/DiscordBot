using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class Invite : ModuleBase<SocketCommandContext>
    {

        [Command("Invite"), RequireUserPermission(GuildPermission.Administrator)]
        [Alias("INVITE", "invite", "inv", "INV")]

        public async Task InviteUserAsync()
        {
            //var arguments = args.Split(' ');
            //var inv = Context.Client.GetInviteAsync("540248332069765128");
            var invite = await Context.Client.GetInviteAsync("CustomInv");
            //var invites = await Context.Guild.GetInvitesAsync();
            await ReplyAsync(invite.Url);
            //ulong.TryParse(arguments[0], out var recipient);
            //if (recipient.ToString().Length > 1)
            //{
            //    var user = Context.Client.GetUser(recipient);
            //    await user.SendMessageAsync("PLACEHOLDER");
            //}
            //else
            //{
            //    EmbedBuilder embed = new EmbedBuilder
            //    {
            //        Title = "It's Dangerous to go alone!",
            //        Description = "FUCKWITS!"
            //    };
            //    await ReplyAsync("", false, embed.Build());
            //}
        }
}
}