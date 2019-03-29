using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Objects;

namespace DiscordBot.Modules
{
    public class ListQuestions : ModuleBase<SocketCommandContext>
    {
        [Command("ListQuestions"), RequireUserPermission(GuildPermission.Administrator)]
        [Alias("LQ", "listquestions", "listQuestions", "Listquesitons")]
        [Summary("Returns a summary of all questions asked to the bot")]
        public async Task ListQuestionsAsync()
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            var active = "";
            var solved = "";
            var numberSolved = 0;
            var numberActive = 0;
            foreach (var q in Program.ActiveQuestions)
            {
                if (q.Solved)
                {
                    solved += ToString(q);
                    numberSolved++;
                }
                else
                {
                    active += ToString(q);
                    numberActive++;
                }
            }

            await Context.Message.Author.SendFileAsync(@"questions.csv", $"Number Active: {numberActive}\nNumber Solved: {numberSolved}\n\nSolved: \n{solved}\n\n Active: \n{active}");
        }

        private static string ToString(Question q)
        {
            return $"ID: {q.Id}\tCreated: {q.Time}\tUser ID: {q.UserId}\tAssigned: {q.AssignedTo}\n";
        }
    }

}