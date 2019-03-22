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
            _path = @"userRegistration_" + today + ".txt";
            using (StreamWriter sw = File.CreateText(_path))
            {
                sw.WriteLine("User registration " + today);
            }
            List<SocketUser> currentUsers = new List<SocketUser>();
            List<ulong> userIds = new List<ulong>();
            foreach (var user in Program.GetServer.Users)
            {
                if (user.VoiceChannel != null)
                {
                    currentUsers.Add(user);
                    userIds.Add(user.Id);
                    _message += $"{user.Id} \t\tChannel: {user.VoiceChannel} \tUser: {user.Username} \t\t\tNick: {user.Nickname} \n";
                }
            }

            _message += $"Number of users: {currentUsers.Count}";
            using (StreamWriter sw = File.AppendText(_path))
            {
                sw.WriteLine(_message);
            }
            return new Tuple<string, List<ulong>>(_path, userIds);
        }
    }
}