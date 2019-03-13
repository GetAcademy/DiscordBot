using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MyBot.Modules
{
    public class Fetch : ModuleBase<SocketCommandContext>
    {
        [Command("Fetch"), RequireUserPermission(GuildPermission.ManageChannels)]
        [Alias("fetch", "FETCH")]
        [Summary("Fetch a question with the given ID")]
        public async Task FetchAsync(string id)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            EmbedBuilder builder = new EmbedBuilder();
            long.TryParse(id, out var parsedId);
            foreach (var question in MyBot.Program.ActiveQuestions)
            {
                if (question.Id == parsedId)
                {
                    builder.WithTitle("Spørsmål")
                        .WithDescription(question.Content + "\n" + question.HowToRepeat)
                        .AddField("Brukernavn: ", MyBot.Program.Guild.GetUser(question.UserId))
                        .AddField("Spørsmål ID", question.Id)
                        .AddField("Dato", question.Time)
                        .AddField("Assigned Teacher:", MyBot.Program.Guild.GetUser(question.AssignedTo));
                    break;
                }
            }
            await Context.User.SendMessageAsync("", false, builder.Build());
        }
    }
}