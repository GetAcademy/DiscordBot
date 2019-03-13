using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class AllQuestions : ModuleBase<SocketCommandContext>
    {
        [Command("AllQuestions"), RequireUserPermission(GuildPermission.ManageChannels)]
        [Alias("AQ")]
        public async Task AllQuestionsAsync()
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            await Context.User.SendFileAsync(@"questions.csv");
        }
    }
}