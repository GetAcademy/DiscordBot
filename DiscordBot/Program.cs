using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LogMessage = Discord.LogMessage;


namespace MyBot
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().RunBotAsync().GetAwaiter().GetResult();

        #region RoleIDs //REPLACE WITH NEW VALUES
        private readonly ulong _admin = 552055706791182347; //ADMIN ROLE ID
        private readonly ulong _startIT = 544423343978315777; // START IT ROLE ID
        private readonly ulong _student = 552473233493065748; // STUDENT ROLE ID
        private readonly ulong _teacher = 552616175415328780; // STUDENT ROLE ID
        #endregion

        #region text channels //REPLACE WITH NEW VALUES
        private readonly ulong _general = 540248332069765134;
        private readonly ulong _bot = 550960388376756224;
        private readonly ulong _errors = 550982436574593044;
        private readonly ulong _startIT_general = 552165841261690901;
        private readonly ulong _team1 = 552166063723249666;
        private readonly ulong _team2 = 552166088646066189;
        #endregion

        #region voice channels //REPLACE WITH NEW VALUES
        private readonly ulong _generalVoice = 552166007767171083;
        private readonly ulong _team1Voice = 552166126403190795;
        private readonly ulong _team2Voice = 552166146816606229;
        #endregion

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private static readonly string path = @"logfile.txt";
        private readonly ulong _serverName = 540248332069765128;
        private readonly string _botToken = File.ReadAllLines(@"I:\GET\DiscordBot\token.txt")[0];

        #region channel objects
        public static SocketTextChannel GeneralChannel;
        public static SocketTextChannel StartItGeneralTextChannel;
        public static SocketTextChannel BotChannel;
        public static SocketTextChannel ErrorChannel;
        public static SocketTextChannel Team1TextChannel;
        public static SocketTextChannel Team2TextChannel;

        public static SocketVoiceChannel Team1VoiceChannel;
        public static SocketVoiceChannel Team2VoiceChannel;
        public static SocketVoiceChannel StartItGeneralVoiceChannel;
        #endregion
        
        public static SocketGuild _guild;

        public async Task RunBotAsync()
        {
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("LOGFILE GET BOT");
                }
            }

            Logging("\n\nGETsharp Bot startup");

            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            #region client Event handler subscriptions

            _client.Log += Log; // Adds the local Log() Event handler to the client.
            _client.UserJoined += AnnounceUserJoined; //Add event handler to client.
            _client.MessageDeleted += MessageDeleted;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += ReplyUserDmAsync;
            _client.UserLeft += HandleUserLeaveAsync;
            _client.GuildMemberUpdated += ReportMemberUpdateAsync;
            

            #endregion


            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, _botToken);
            await _client.StartAsync();
            await Task.Delay(-1);
        }


        #region Tasks
        private Task ReportMemberUpdateAsync(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            // Logs and reports to BotChannel of the changes done to the guild member
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            GeneralChannel = _client.GetGuild(_serverName).GetTextChannel(_general);
            BotChannel = _client.GetGuild(_serverName).GetTextChannel(_bot);
            ErrorChannel = _client.GetGuild(_serverName).GetTextChannel(_errors);
            StartItGeneralTextChannel = _client.GetGuild(_serverName).GetTextChannel(_startIT_general);
            StartItGeneralVoiceChannel = _client.GetGuild(_serverName).GetVoiceChannel(_generalVoice);

            Team1TextChannel = _client.GetGuild(_serverName).GetTextChannel(_team1);
            Team1VoiceChannel = _client.GetGuild(_serverName).GetVoiceChannel(_team1Voice);
            Team2TextChannel = _client.GetGuild(_serverName).GetTextChannel(_team2);
            Team2VoiceChannel = _client.GetGuild(_serverName).GetVoiceChannel(_team2Voice);

            _guild = _client.GetGuild(_serverName);
            BotChannel.SendMessageAsync("GETsharp Bot is running!");
            return Task.CompletedTask;
        }

        private Task HandleUserLeaveAsync(SocketGuildUser arg)
        {
            SendMessageBotChannel($"User: {arg.Username} Left the server", "User Left", "Automatic");
            return Task.CompletedTask;
        }

        private async Task ReplyUserDmAsync(SocketMessage msg)
        {
            if (msg.Channel.Name != "general" && !msg.Author.IsBot) //Message is DM
            {
                var role = "";
                Logging($"Message recieved from: {msg.Author.Username} id: {msg.Author.Id}\nContent: {msg.Content}");
                Console.WriteLine("Revieved DM from: " + msg.Channel.Name);
                IReadOnlyCollection<SocketRole> userRoles = _guild.GetUser(msg.Author.Id).Roles;
                //The roles "ADMIN", "TEACHER" and "STUDENT" must be EXCLUSIVE!!!
                if (userRoles.Count > 0)
                {
                    //Console.WriteLine("User has roles!");
                    foreach (var userRole in userRoles)
                    {
                        //Console.WriteLine(userRole.Name);
                        switch (userRole.Name.ToString())
                        {
                            case "ADMIN":
                                Console.WriteLine("ADMIN ACC");
                                role = "ADMIN ";
                                break;
                            case "STUDENT":
                                Console.WriteLine("STUDENT ACC");
                                role = "STUDENT";
                                break;
                            case "TEACHER":
                                Console.WriteLine("TEACHER ACC");
                                role = "TEACHER";
                                break;
                        }
                    }
                }
                if (msg.Content.Split(' ')[0].ToLower().Contains("!info")) //DM message says !info
                {
                    var report = $"Replying to user: {msg.Author.Username}\n";
                    SendMessageBotChannel(report, "Reply", "Automatic");
                    Logging(report);
                    await msg.Author.SendMessageAsync(
                        "Heisann! Her kommer det mer info etterhvert. Work in progress ;)");
                }
                else
                {
                    switch (role)
                    {
                        //if student, forward to teachers
                        case "STUDENT":
                            break;

                        // if Admin this may be a command to the bot
                        case "ADMIN":
                            break;

                        // if Teacher this message may be a broadcast to students
                        case "TEACHER":
                            break;
                        default:
                            SendMessageBotChannel("Unknown user replying to bot", "LOG", "Server");
                            Logging($"Unregistered user {msg.Author.Username} attempring to communicate with bot: \n{msg.Content}");
                            break;
                    }
                }
            }
        }

        private Task MessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            SendMessageBotChannel($"Message ID: {arg1.Id} Deleted", "Deletion", "Automatic");
            Logging("Message ID: " + arg1.Id + " Deleted");
            return Task.CompletedTask;
        }


        private async Task AnnounceUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;
            var channel = guild.DefaultChannel;
            Logging($"UserID: {user.Id} Joined server");
            Console.WriteLine($"UserID: {user.Id} Joined server");
            EmbedBuilder build = new EmbedBuilder
            {
                Description =
                      $"I kurset kommer vi til å bruke [Github](https://github.com/) til å samle koden dere lager og til hosting av nettsider når vi bygger dem.\n" +
                      $"For å skrive koden står dere fritt til å velge IDE/teksteditor, men vi anbefaler å bruke " +
                      $"[Visual Studio](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=15).\n" +
                      $"Har du en eldre PC kan det være nyttig å bruke [Visual Studio Code](https://code.visualstudio.com/docs?dv=win&wt.mc_id=DX_841432&sku=codewin)\n" +
                      $"Kurset har oppgaver og informasjon på [Moodle](https://getacademy.moodlecloud.com/)\n" +
                      $"Informasjon og oppgaver vil også bli gitt på [Google Classroom](https://classroom.google.com/)"
            };

            await user.SendMessageAsync(
                $"Heisann, {user.Username}! Og velkommen til START IT! Jeg er {_client.CurrentUser.Mention} " +
                $"og er en robot! \nJeg kommer til å være tilgjengelig i chatten \"General\" og kommer til å gi dere " +
                $"påminnelser og informasjon i kurset utover Våren/høsten." +
                $"\n\nHvis du har noen spørsmål, ta gjerne kontakt med:\n" +
                $"\t{_client.GetUser(268754579988938752).Mention} Lærer i IT-utvikling\n" +
                $"\t{_client.GetUser(363256000800751616).Mention} Lærer i Nøkkelkompetanse\n" +
                $"\t{_client.GetUser(112955646701297664).Mention} Hjelpelærer/Discord ansvarlig\n\n" +
                $"Ønsker du mer informasjon fra meg så kan du svare på denne meldingen ved å skrive \"!info\", eller tagge meg og gi kommandoen \"help\" i General."
                , false, build.Build());

            await channel.SendMessageAsync($"Velkommen til General, {user.Mention}!");
            //await user.AddRoleAsync(_guild.GetRole(_startIT));

        }

        private static Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Logging($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        // Handles incoming message on the server
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            Logging(arg.ToString());
            if (message is null || message.Author.IsBot) return;
            var argPos = 0;
            if (message.HasStringPrefix("bot!", ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                Console.WriteLine(message);
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    Logging("!!!ERROR!!!\t" + result.ErrorReason);
                    SendError(message, result);
                }
            }
        }
        #endregion


        #region Static Methods

        private static void Logging(string message)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(message);
            }

        }

        public static void SendMessageBotChannel(string result, string action, string user, SocketUserMessage message = null)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Blue)
                .WithCurrentTimestamp()
                .AddField("Action:", action)
                .AddField("User: ", user)
                .AddField("Result: ", result);
            BotChannel.SendMessageAsync("", false, builder.Build());
        }

        private void SendError(SocketUserMessage message, IResult result)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("ERROR")
                .AddField("Invoking message", message.Content)
                .AddField("Invoking user", message.Author)
                .WithDescription(result.ErrorReason)
                .WithCurrentTimestamp()
                .WithColor(Color.Red);
            ErrorChannel.SendMessageAsync("", false, builder.Build());
        }


    }

    #endregion

}

