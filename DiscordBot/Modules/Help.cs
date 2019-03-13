using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("Help")]
        [Alias("help", "HELP", "?")]
        public async Task HelpAsync()
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            EmbedBuilder builder = new EmbedBuilder();
            builder.AddField("Ping", "Pong!")
                .AddField("HowTo", "Invoke the bot by using the command bot!<command> or by tagging the bot and a command.")
                .WithTitle("HELP text for GETsharp Discord Bot")
                .AddField("Log", "Sends the bot log file as dm to invoker. (ADMIN)")
                .AddField("Echo", "Echoes the users input")
                .AddField("PM", "Sends a private message to a given username. (ADMIN)\n" +
                                "Use: @GET PM [UserID/\"Username\"] <message>")
                .AddField("Invite", "Sends a server invite link as pm. If no user id is given, the bot will post in general (ADMIN)\n" +
                                    "Use: @GET Invite [UserID]")
                .AddField("Boilerplate", "Sends a boilerplate file back to the user as dm\n" +
                                         "Usage: @GET Boilerplate [lang] attributes\n" +
                                         "Example: @GET Boilerplate HTML -L //Sends a linked set of files (html, css and js as .rar file)")
                .WithColor(Color.Red)
                .WithImageUrl("https://i.chzbgr.com/full/8332814592/h5D460AE7/");

            
            await ReplyAsync("", false, builder.Build());
        }
    }
}