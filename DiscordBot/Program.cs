using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyBot.Modules;
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

        #region GET_Server

            public static SocketGuild GetServer;
            public static SocketTextChannel GetServerGeneralChannel;
            private readonly ulong _GET_server = 349263724856737792;
            private readonly ulong _GET_general = 491966014142021633;

            #region startIT4

                private readonly ulong _startIT4_Category = 537930722393194504;
                private readonly ulong _startIT4_general = 538289968007610379;
                private readonly ulong _startIT4_general_voice = 538290021153767451;

                private readonly ulong _startIT4_team1 = 539895437147111425;
                private readonly ulong _startIT4_team2 = 542680345850544128;
                private readonly ulong _startIT4_team3 = 539895590104858635;
                private readonly ulong _startIT4_team4 = 539895614419369994;
                private readonly ulong _startIT4_team5 = 549911410382209035;
                private readonly ulong _startIT4_team6 = 550637555360595988;

                private readonly ulong _startIT4_team1_voice = 539895757289816074;
                private readonly ulong _startIT4_team2_voice = 539896033631797248;
                private readonly ulong _startIT4_team3_voice = 539896053231648771;
                private readonly ulong _startIT4_team4_voice = 539896078057603074;
                private readonly ulong _startIT4_team5_voice = 539896102787219456;
                private readonly ulong _startIT4_team6_voice = 550613975067262977;

                public static SocketCategoryChannel StartIt4CategoryChannel;
                public static SocketTextChannel StartIt4GeneralTextChannel;
                public static SocketVoiceChannel StartIt4GeneralVoiceChannel;

                public static SocketTextChannel StartIt4Team1TextChannel;
                public static SocketTextChannel StartIt4Team2TextChannel;
                public static SocketTextChannel StartIt4Team3TextChannel;
                public static SocketTextChannel StartIt4Team4TextChannel;
                public static SocketTextChannel StartIt4Team5TextChannel;
                public static SocketTextChannel StartIt4Team6TextChannel;

                public static SocketVoiceChannel StartIt4Team1VoiceChannel;
                public static SocketVoiceChannel StartIt4Team2VoiceChannel;
                public static SocketVoiceChannel StartIt4Team3VoiceChannel;
                public static SocketVoiceChannel StartIt4Team4VoiceChannel;
                public static SocketVoiceChannel StartIt4Team5VoiceChannel;
                public static SocketVoiceChannel StartIt4Team6VoiceChannel;


            #endregion

            #region startIT5

            

            #endregion

        #endregion



        #region voice channels //REPLACE WITH NEW VALUES
        private readonly ulong _generalVoice = 552166007767171083;
        private readonly ulong _team1Voice = 552166126403190795;
        private readonly ulong _team2Voice = 552166146816606229;
        #endregion

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        #region Fields

        private Timer timer;

        private static readonly string path = @"logfile.txt";
        private static readonly string _userPath = @"users.txt";
        private static readonly string _daemonPath = @"crashHandler.exe";
        public static List<Question> ActiveQuestions = new List<Question>();
        private readonly ulong _serverName = 540248332069765128;
        private readonly string _botToken = File.ReadAllLines(@"E:\GET\DiscordBot\token.txt")[0];


        #endregion

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
        
        public static SocketGuild Guild;

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

            if (!File.Exists(_userPath))
            {
                using (StreamWriter sw2 = File.CreateText(_userPath))
                {
                    sw2.WriteLine("Brukere som har blitt registrert\n");
                }
            }
            timer = new Timer(AnnounceFridayReminder);

            // Figure how much time until 12:00
            DateTime now = DateTime.Now;
            DateTime twelveOClock = DateTime.Today.AddHours(12.0);

            // If it's already past 12:00    
            if (now > twelveOClock)
            {
                twelveOClock = twelveOClock.AddDays(1.0);
            }

            int msUntilFour = (int)((twelveOClock - now).TotalMilliseconds);

            // Set the timer to once per day.
            timer.Change(msUntilFour, 86400000);

            ActiveQuestions = LoadData.ReadQuestions(); //Load all stored questions into memory
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
            //_client.GuildMemberUpdated += ReportMemberUpdateAsync;
            _client.UserVoiceStateUpdated += HandleUserVoiceActionAsync;

            #endregion


            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, _botToken);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private void AnnounceFridayReminder(object state)
        {
            GeneralChannel.SendMessageAsync("Klokken er nå 12:00");
        }


        #region Tasks

        private Task HandleUserVoiceActionAsync(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            Console.WriteLine($"User {user.Username} {user.Id}\nMoved from: {before.VoiceChannel.Name}\nMoved to: {after.VoiceChannel.Name}");
            return Task.CompletedTask;
        }

        private Task ReportMemberUpdateAsync(SocketGuildUser before, SocketGuildUser after)
        {
            // Logs and reports to BotChannel of the changes done to the guild member
            // Currently this code also reports user status and online/offline actions!!!!!
            Console.WriteLine($"Change to user: {after.Username}\nAction: {after.Hierarchy}");
            return Task.CompletedTask;
        }


        private Task ReadyAsync()
        {
            #region Testing Server
            GeneralChannel = _client.GetGuild(_serverName).GetTextChannel(_general);
            BotChannel = _client.GetGuild(_serverName).GetTextChannel(_bot);
            ErrorChannel = _client.GetGuild(_serverName).GetTextChannel(_errors);
            StartItGeneralTextChannel = _client.GetGuild(_serverName).GetTextChannel(_startIT_general);
            StartItGeneralVoiceChannel = _client.GetGuild(_serverName).GetVoiceChannel(_generalVoice);

            Team1TextChannel = _client.GetGuild(_serverName).GetTextChannel(_team1);
            Team1VoiceChannel = _client.GetGuild(_serverName).GetVoiceChannel(_team1Voice);
            Team2TextChannel = _client.GetGuild(_serverName).GetTextChannel(_team2);
            Team2VoiceChannel = _client.GetGuild(_serverName).GetVoiceChannel(_team2Voice);

            Guild = _client.GetGuild(_serverName); //Server object
            #endregion

            #region MyRegion

            GetServer = _client.GetGuild(_GET_server); // GET server
            GetServerGeneralChannel = GetServer.GetTextChannel(_GET_general); // General text channel

            StartIt4Team1TextChannel = GetServer.GetTextChannel(_startIT4_team1);
            StartIt4Team2TextChannel = GetServer.GetTextChannel(_startIT4_team2);
            StartIt4Team3TextChannel = GetServer.GetTextChannel(_startIT4_team3);
            StartIt4Team4TextChannel = GetServer.GetTextChannel(_startIT4_team4);
            StartIt4Team5TextChannel = GetServer.GetTextChannel(_startIT4_team5);
            StartIt4Team6TextChannel = GetServer.GetTextChannel(_startIT4_team6);

            StartIt4Team1VoiceChannel = GetServer.GetVoiceChannel(_startIT4_team1_voice);
            StartIt4Team2VoiceChannel = GetServer.GetVoiceChannel(_startIT4_team2_voice);
            StartIt4Team3VoiceChannel = GetServer.GetVoiceChannel(_startIT4_team3_voice);
            StartIt4Team4VoiceChannel = GetServer.GetVoiceChannel(_startIT4_team4_voice);
            StartIt4Team5VoiceChannel = GetServer.GetVoiceChannel(_startIT4_team5_voice);
            StartIt4Team6VoiceChannel = GetServer.GetVoiceChannel(_startIT4_team6_voice);


            #endregion


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
            //Console.WriteLine(msg.Channel.Name);
            var name = String.Copy(msg.Channel.Name);
            name = name.Substring(1, name.Length - 6);
            //Console.WriteLine($"{msg.Author.Username} == {name}");
            if (msg.Author.Username == name && !msg.Author.IsBot) //Message is DM
            {
                var role = "";
                Logging($"Message recieved from: {msg.Author.Username} id: {msg.Author.Id}\nContent: {msg.Content}");
                Console.WriteLine("Revieved DM from: " + msg.Channel.Name);
                
                IReadOnlyCollection<SocketRole> userRoles = Guild.GetUser(msg.Author.Id).Roles;
                //The roles "ADMIN", "TEACHER" and "STUDENT" must be EXCLUSIVE!!!
                var done = false;
                if (userRoles.Count > 0)
                {
                    //Console.WriteLine("User has roles!");
                    foreach (var userRole in userRoles)
                    {
                        //if (done) continue;
                        switch (userRole.Name.ToString())
                        {
                            
                            //if student, Create a question object
                            case "STUDENT":
                                SendMessageBotChannel($"User Role: {userRole.Name} replying to bot", "LOG", "Server");
                                if (msg.Content.Contains("!question"))
                                {
                                    var q = CreateQuestion(msg);
                                    //var q = new Question(msg.Content, "null");
                                    Console.WriteLine("Made object");
                                    q.AddToFile();
                                    ActiveQuestions.Add(q);
                                }
                                else
                                {
                                    await msg.Author.SendMessageAsync("Heisann! Hvis du har ett spørsmål til oss, svar med " +
                                                                "!question \"[Spørsmålet ditt] | [Hvordan vi kan gjenskape problemet]\"" +
                                                                "\n __**eksempel:**__ \n!question Hvordan sender jeg kommandoer til botten | Prøver å sende en kommando, men det skjer ingen ting\n" +
                                                                "**!NB!** Husk å ha med \"|\" mellom hver del av spørsmålet du skal sende! ;)");
                                }

                                //done = true;
                                break;

                            // if Admin this may be a command to the bot
                            case "ADMIN":
                                SendMessageBotChannel($"User Role: {userRole.Name} replying to bot", "LOG", "Server");
                                //done = true;
                                break;

                            // if Teacher this message may be a broadcast to students
                            case "TEACHER":
                                SendMessageBotChannel($"User Role: {userRole.Name} replying to bot", "LOG", "Server");
                                if (msg.Content.Contains("?Q"))
                                {
                                    if (ActiveQuestions.Any(question => !question.Solved))
                                    {
                                        if (ActiveQuestions.Any(question => (question.AssignedTo != 0)))
                                        {
                                            await msg.Author.SendMessageAsync(
                                                "All active questions have been assigned to a teacher");
                                            break;
                                        }
                                        foreach (var question in ActiveQuestions)
                                        {
                                            if (!question.Solved && question.AssignedTo == ulong.MinValue)
                                            {
                                                var builder = new EmbedBuilder
                                                {
                                                    Color = Color.Green,
                                                    Description = question.Content + "\n" + question.HowToRepeat
                                                };
                                                builder.AddField("Brukernavn: ",
                                                        GetServer.GetUser(question.UserId).Username)
                                                    .AddField("Spørsmål ID", question.Id)
                                                    .AddField("Dato", question.Time)
                                                    .AddField("Svar på spørsmålet ved å skrive:",
                                                        $"?SOLVE {question.Id}");
                                                Console.WriteLine($"Sent question to: {msg.Author.Username}");
                                                question.AssignedTo = msg.Author.Id;
                                                question.WriteToFile();
                                                await msg.Author.SendMessageAsync("", false, builder.Build());
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await msg.Author.SendMessageAsync(
                                            "Det ser ut til at det er ingen flere aktive spørsmål! :)");

                                    }
                                }
                                else if (msg.Content.Contains("?questions"))
                                {
                                    var active = 0;
                                    foreach (var q in ActiveQuestions)
                                    {
                                        active += q.Solved ? 0 : 1;
                                    }
                                    await msg.Author.SendMessageAsync($"Antall aktive spørsmål: {active}");
                                }
                                else if (msg.Content.Contains("?solve") || msg.Content.Contains("?SOLVE"))
                                {
                                    var parts = msg.Content.Split(' ');
                                    long.TryParse(parts[1], out var questionId);
                                    foreach (var question in ActiveQuestions)
                                    {
                                        if (questionId != question.Id) continue;
                                        question.Solved = true;
                                        await msg.Author.SendMessageAsync($"Spørsmål ID {question.Id} Løst!");
                                        break;
                                    }

                                    foreach (var q in ActiveQuestions)
                                    {
                                        q.WriteToFile();
                                    }
                                }
                                else if(msg.Content.Contains("!BROADCAST"))
                                {
                                    var message = msg.Content.Substring(10);
                                    foreach (var user in Guild.Users)
                                    {
                                        if (user.Roles.Any(r => r.ToString() == "STUDENT"))
                                        {
                                            await user.SendMessageAsync($"Dette er en broadcast melding fra {msg.Author.Username}\n" + message);
                                        }
                                    }
                                }
                                else
                                {
                                    await msg.Author.SendMessageAsync(
                                        "Heisann! Jeg forsto ikke helt den kommandoen... \n" +
                                        "Hvis du vil ha en oversikt over aktive spørsmål, send ?questions\n" +
                                        "Hvis du vil at jeg skal sende deg ett spørsmål, svar med ?Q\n" +
                                        "Hvis du vil sende en melding til alle registrerte studentder, svar med !BROADCAST [Melding]");
                                }

                                //done = true;
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
                        "Heisann! Her kommer det mer info etterhvert. Work in progress ;)"
                        );
                } else if (msg.Content.Split(' ').Contains("!navn"))
                {
                    var message = msg.Content.Split(',');
                    var firstName = message[1];
                    var lastName = message[2];
                    var id = msg.Author.Id;
                    var username = msg.Author.Username;
                    using (StreamWriter sw2 = File.AppendText(_userPath))
                    {
                        sw2.WriteLine($"{id},{lastName},{firstName},{username}");
                    }

                    await msg.Author.SendMessageAsync("Flott! Nå er du registrert!");
                }
            }
        }

        private Question CreateQuestion(SocketMessage msg)
        {
            var values = msg.Content.Split('|');
            var content = values[0] == "!question" ?  values[1] : values[0];
            var howTo = values[0] == "!question" ?  values[2] : values[1];
            msg.Author.SendMessageAsync("Takk! Da har jeg registrert spørsmålet!");
            EmbedBuilder builder = new EmbedBuilder
            {
                Title = "Spørsmål",
                Color = Color.DarkTeal,
                Description = content + "\n" + howTo,
            };
            msg.Author.SendMessageAsync("", false, builder.Build());
            return new Question(msg.Author.Id, content.Substring(9), howTo);   
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
                $"Ønsker du mer informasjon fra meg så kan du svare på denne meldingen ved å skrive \"!info\", eller tagge meg og gi kommandoen \"help\" i General.\n\n" +
                $"Du må nå svare på denne meldingen ved å skrive [!navn \"Fornavnet ditt\" \"Etternavn\"] for å bli registrert for meg! :) eksempel: \"!navn Navn Navnesen\""
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
            if (message is null || message.Author.IsBot || message.Channel.Name != "general") return;
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

