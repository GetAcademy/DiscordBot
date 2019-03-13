using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class Boilerplate : ModuleBase<SocketCommandContext>
    {
        [Command("Boilerplate")]
        [Alias("boilerplate", "BP", "bp")]
        public async Task BoilerplateAsync([Remainder] string remainder)
        {
            var filePath = "";
            if (remainder.Contains("HTML") && remainder.Contains("-F"))
            {
                filePath = @"..\..\Templates\full.html";
            }
            else if (remainder.Contains("HTML") && remainder.Contains("-L"))
            {
                filePath = @"..\..\Templates\linked\linked.rar";
            }
            else if (remainder.Contains("HTML") && remainder.Contains("-B"))
            {
                filePath = @"..\..\Templates\basic.html";
            }
            else if (remainder.Contains("HTML") && remainder.Length < 5)
            {
                filePath = @"..\..\Templates\empty.html";
            } else if (remainder.Contains("CSS"))
            {
                filePath = @"..\..\Templates\empty.css";
            }
            else if (remainder.Contains("JS") && remainder.Contains("-C"))
            {
                filePath = @"..\..\Templates\class.js";
            }
            else if (remainder.Contains("JS") && remainder.Length < 3)
            {
                filePath = @"..\..\Templates\empty.js";
            }

            await Context.User.SendFileAsync(filePath, "Here are the files you requested");
        }
    }
}