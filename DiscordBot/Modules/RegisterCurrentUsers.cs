using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MyBot.Modules
{
    public class RegisterCurrentUsers : ModuleBase<SocketCommandContext>
    {
        private string _path;
        private string _message;
        [Command("RegisterCurrentUsers"), RequireUserPermission(ChannelPermission.ManageChannels)]
        [Alias("RCU", "RegisterUsers")]
        public async Task RegisterCurrentUsersAsync()
        {
            var today = DateTime.Now.ToString("dd/MM/yyyy");
            _path = @"userRegistration_" + today + ".txt";
            //if (!File.Exists(_path))
            //{
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(_path))
                {
                    sw.WriteLine("User registration " + today);
                }
            //}
            List<SocketUser> currentUsers = new List<SocketUser>();
            List<ulong> userIds = new List<ulong>();
            foreach (var user in Context.Guild.Users)
            {
                if (user.VoiceChannel != null)
                {
                    currentUsers.Add(user);
                    userIds.Add(user.Id);
                    _message += $"User: {user.Username} id: {user.Id} in channel: {user.VoiceChannel} \n";
                }
            }
            using (StreamWriter sw = File.AppendText(_path))
            {
                sw.WriteLine(_message);
            }

            await ReplyAsync("Registered!");
            await MyBot.Program.BotChannel.SendFileAsync(_path, "");
        }
    }
}