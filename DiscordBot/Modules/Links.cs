using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class Links : ModuleBase<SocketCommandContext>
    {
        [Command("Links")]
        public async Task LinksAsync()
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            EmbedBuilder builder = new EmbedBuilder
            {
                Title = "LINKS!",
                Color = Color.Green,
                Description = $"Her er en samling med linker som dere vil trenge i kurset\n" +
                              $"[MOODLE](https://getacademy.moodlecloud.com)\n" +
                              $"[GITHUB](https://github.com/GetAcademy/)\n" +
                              $"[GOOGLE CLASSROOM](https://classroom.google.com/)\n",
                
            };
            builder.WithImageUrl(
                "https://upload.wikimedia.org/wikipedia/en/thumb/0/04/Navi_%28The_Legend_of_Zelda%29.png/220px-Navi_%28The_Legend_of_Zelda%29.png");
            
            await ReplyAsync("", false, builder.Build());
        }
    }
}