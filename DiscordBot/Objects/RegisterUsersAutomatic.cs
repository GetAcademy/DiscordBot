using System;
using System.Collections.Generic;
using System.IO;
using Discord.WebSocket;

namespace DiscordBot.Objects
{
    public class RegisterUsersAutomatic
    {
        private static string _path;
        private static string _message;
        public static Tuple<string, List<ulong>> Register()
        {
            var today = DateTime.Now.ToString("dd/MM/yyyy");
            _path = @"userRegistration_" + today + ".csv";
            var lines = new List<string>();
            
            var currentUsers = new List<SocketUser>();
            var userIds = new List<ulong>();
            using (var sw = File.CreateText(_path))
            {
                foreach (var user in Program.GetServer.Users)
                {
                    if (user.VoiceChannel == null || user.VoiceChannel.Category.Id != Program.DefaultCategory) continue;
                    currentUsers.Add(user);
                    userIds.Add(user.Id);
                    sw.WriteLine($"{user.Id},{user.VoiceChannel},{user.Username},{user.Nickname}");
                }
            }


            //_message += $"Number of users: {currentUsers.Count}";
            return new Tuple<string, List<ulong>>(_path, userIds);
        }
    }
}