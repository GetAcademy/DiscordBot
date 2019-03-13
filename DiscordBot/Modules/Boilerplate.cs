using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class Boilerplate : ModuleBase<SocketCommandContext>
    {
        [Command("Boilerplate")]
        [Alias("boilerplate", "BP", "bp")]
        public async Task BoilerplateAsync(string lang, [Remainder] string remainder = " ")
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            var err = false;
            var filePath = "";
            if ((lang == "HTML" || lang == "html") && remainder.Contains("-F"))
            {
                filePath = @"..\..\Templates\full.html";
            }
            else if ((lang == "HTML" || lang == "html") && remainder.Contains("-L"))
            {
                filePath = @"..\..\Templates\linked\linked.rar";
            }
            else if ((lang == "HTML" || lang == "html") && remainder.Contains("-B"))
            {
                filePath = @"..\..\Templates\basic.html";
            }
            else if ((lang == "HTML" || lang == "html") && remainder.Length < 2)
            {
                filePath = @"..\..\Templates\empty.html";
            } else if (lang == "css" || lang == "CSS")
            {
                filePath = @"..\..\Templates\empty.css";
            }
            else if ((lang == "js" || lang == "JS") && remainder.Contains("-C"))
            {
                filePath = @"..\..\Templates\class.js";
            }
            else if ((lang == "js" || lang == "JS") && remainder.Length < 2)
            {
                filePath = @"..\..\Templates\empty.js";
            }
            else
            {
                await ReplyAsync("Incorrect format. Use the command @GET help for info.");
                return;
                //err = true;
            }

            await Context.User.SendFileAsync(filePath, "Here are the files you requested");
        }
    }
}