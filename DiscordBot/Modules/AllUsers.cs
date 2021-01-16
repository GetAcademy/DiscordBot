using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class GetAllUsers : ModuleBase<SocketCommandContext>
    {
        [Command("GetAllUsers"), RequireUserPermission(GuildPermission.ManageChannels), Alias("AU", "allusers"), Summary("")]

        public async Task GetAllUsersAsync()
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            var output = "";
            var builder = new EmbedBuilder();
            var users = new List<Tuple<ulong, string>>(); // List of tuples containing <UserID, TopRole>
            foreach (var user in Context.Guild.Users)
            {
                string userRole = null;
                var userId = user.Id;
                foreach (var role in user.Roles)
                {
                    switch (role.ToString())
                    {
                        case "STUDENT":
                        case "TEACHER":
                        case "ADMIN":
                            userRole += role.ToString() + "\t";
                            break;
                    }
                }

                users.Add(new Tuple<ulong, string>(userId, userRole));
            }

            foreach (var line in users)
            {
                output += $"{Context.Guild.GetUser(line.Item1).Username} \t {line.Item2}\n";
            }
            Console.Write(output);
            await Context.Message.Author.SendMessageAsync(output);
        }
    }
}