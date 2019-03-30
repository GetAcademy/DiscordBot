using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Objects;
using Microsoft.Extensions.DependencyInjection;
using LogMessage = Discord.LogMessage;


namespace DiscordBot
{
    public class Program
    {
        #region Fields
        #region RoleIDs //REPLACE WITH NEW VALUES
        private readonly ulong _admin = 552055706791182347; //ADMIN ROLE ID
        private readonly ulong _startIT = 544423343978315777; // START IT ROLE ID
        private readonly ulong _student = 552473233493065748; // STUDENT ROLE ID
        private readonly ulong _teacher = 552616175415328780; // STUDENT ROLE ID
        #endregion

        #region text channels //REPLACE WITH NEW VALUES

        private readonly ulong _general = 538289968007610379; //540248332069765134;
        private readonly ulong _bot = 553158666350624789; //550960388376756224;
        private readonly ulong _errors = 553158744012357657; //550982436574593044; 
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

        private static readonly ulong _startIT4_Category = 537930722393194504;
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

        private static DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        public static ulong DefaultCategory = _startIT4_Category; //Set this variable according to the Category the bot shall overview
        //private Timer _timer;
        private TimerAlerts _alerts;
        //public readonly DateTime startTime 
        public static bool StartDebugOn;
        private static readonly string path = @"logfile.txt";
        private static readonly string _userPath = @"users.txt";
        private static readonly string _daemonPath = @"crashHandler.exe";
        public static List<Question> ActiveQuestions = new List<Question>();
        private readonly ulong _serverName = 540248332069765128;
        private readonly string _botToken = File.ReadAllLines(@"F:\GET\DiscordBot\token.txt")[0];

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

        public static SocketGuild Guild;

        #endregion

        public static void Main(string[] args)
            => new Program().RunBotAsync().GetAwaiter().GetResult();


        
        #region Tasks
        public async Task RunBotAsync()
        {
            ShowMessage();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Debug mode? (Y/N)");
            Console.ForegroundColor = ConsoleColor.White;
            SetDebugMode();
            Daemon();
            InitializeFiles();
            ActiveQuestions = LoadData.ReadQuestions(); //Load all stored questions into memory
            InitializeClient();
            AddEventSubscriptions();
            DisplayQuestionsInQueueStatus();

            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, _botToken);
            await _client.StartAsync();
            await Task.Delay(-1);

        }

        private Task HandleUserVoiceActionAsync(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            ShowMessage(user.Username);
            UserActionTest(user, after);
            Console.WriteLine($"User {user.Username} {user.Id}\nMoved from: {before.VoiceChannel.Name}\nMoved to: {after.VoiceChannel.Name}");
            return Task.CompletedTask;
        }

        private Task ReportMemberUpdateAsync(SocketGuildUser before, SocketGuildUser after)
        {
            ShowMessage();
            // Logs and reports to BotChannel of the changes done to the guild member
            // Currently this code also reports user status and online/offline actions!!!!!
            Console.WriteLine($"Change to user: {after.Username}\nAction: {after.Hierarchy}");
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            ShowMessage();

            #region Testing Server
            GeneralChannel = _client.GetGuild(_GET_server).GetTextChannel(_general);
            BotChannel = _client.GetGuild(_GET_server).GetTextChannel(_bot);
            ErrorChannel = _client.GetGuild(_GET_server).GetTextChannel(_errors);
            StartItGeneralTextChannel = _client.GetGuild(_serverName).GetTextChannel(_startIT_general);
            StartItGeneralVoiceChannel = _client.GetGuild(_serverName).GetVoiceChannel(_generalVoice);

            Team1TextChannel = _client.GetGuild(_serverName).GetTextChannel(_team1);
            Team1VoiceChannel = _client.GetGuild(_serverName).GetVoiceChannel(_team1Voice);
            Team2TextChannel = _client.GetGuild(_serverName).GetTextChannel(_team2);
            Team2VoiceChannel = _client.GetGuild(_serverName).GetVoiceChannel(_team2Voice);

            Guild = _client.GetGuild(_serverName); //Server object
            #endregion

            #region GETserver

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
            return Task.CompletedTask;
        }

        private Task HandleUserLeaveAsync(SocketGuildUser arg)
        {
            ShowMessage();
            SendMessageBotChannel($"User: {arg.Username} Left the server", "User Left", "Automatic");
            return Task.CompletedTask;
        }

        private async Task ReplyUserDmAsync(SocketMessage msg)
        {
            ShowMessage();
            var name = String.Copy(msg.Channel.Name);
            name = name.Substring(1, name.Length - 6);
            if (msg.Author.Username == name && !msg.Author.IsBot) //Message is DM
            {
                //var role = "";
                Logging($"Message recieved from: {msg.Author.Username} id: {msg.Author.Id}\nContent: {msg.Content}");
                Console.WriteLine("Revieved DM from: " + msg.Channel.Name);
                
                IReadOnlyCollection<SocketRole> userRoles = Guild.GetUser(msg.Author.Id).Roles;
                //The roles "ADMIN", "TEACHER" and "STUDENT" must be EXCLUSIVE!!!
                if (msg.Content.Split(' ')[0].ToLower().Contains("!info")) //DM message says !info
                {
                    await ReplyInfo(msg);
                }
                else if (msg.Content.Split(' ').Contains("!navn"))
                {
                    await RegisterName(msg);
                }

                if (userRoles.Count > 0)
                {
                    var roles = "";
                    foreach (var userRole in userRoles)
                    {
                        switch (userRole.Name)
                        {
                            //if student, Create a question object
                            case "STUDENT":
                                roles += userRole + ", ";
                                await ReplyStudent(msg, userRole);
                                break;

                            // if Admin this may be a command to the bot
                            case "ADMIN":
                                roles += userRole.Name + ", ";
                                break;

                            // if Teacher this message may be a broadcast to students
                            case "TEACHER":
                                roles += userRole.Name + ", ";
                                await ReplyTeacher(msg, userRole);
                                break;
                        }
                    }
                    SendMessageBotChannel($"User Role: {roles} replying to bot", "LOG", "Server");
                }


            }
        }

        private static async Task RegisterName(SocketMessage msg)
        {
            ShowMessage();
            var message = msg.Content.Split(',');
            var firstName = message[1];
            var lastName = message[2];
            var id = msg.Author.Id;
            var username = msg.Author.Username;
            //using (var sw2 = File.AppendText(_userPath))
            //{
            //    sw2.WriteLine($"{id},{lastName},{firstName},{username}");
            //}
            //await AssignRole(GetServer.GetUser(msg.Author.Id));
            await msg.Author.SendMessageAsync($"Flott! Nå er du registrert!\n **Er dette korrekt?**" +
                                              $"\n\nFornavn: {firstName} Etternavn: {lastName}\n\n__**Hvis dette er feil," +
                                              $" ta kontakt med {GetServer.GetUser(112955646701297664).Mention}**__");
        }

        private static async Task ReplyInfo(SocketMessage msg)
        {
            ShowMessage();
            var report = $"Replying to user: {msg.Author.Username}\n";
            SendMessageBotChannel(report, "Reply", "Automatic");
            Logging(report);
            await msg.Author.SendMessageAsync(
                "Heisann! Her kommer det mer info etterhvert. Work in progress ;)"
            );
        }

        private static async Task ReplyTeacher(SocketMessage msg, SocketRole userRole)
        {
            ShowMessage();
            
            if (msg.Content.Contains("?Q"))
            {
                if (ActiveQuestions.Any(question => !question.Solved))
                {
                    if (!ActiveQuestions.Any(question => (question.AssignedTo != 0)))
                    {
                        await msg.Author.SendMessageAsync(
                            "All active questions have been assigned to a teacher");
                        return;
                    }

                    foreach (var question in ActiveQuestions)
                    {
                        if (!question.Solved && question.AssignedTo == ulong.MinValue)
                        {
                            await ReplyWithQuestion(msg, question);
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
                await ListActiveQuestions(msg);
            }
            else if (msg.Content.Contains("?solve") || msg.Content.Contains("?SOLVE"))
            {
                await SolveQuestion(msg);
            }
            else if (msg.Content.Contains("!BROADCAST"))
            {
                await BroadcastMessage(msg);
            }
            else
            {
                await ReplyUnknownCommand(msg);
            }
        }

        private static async Task BroadcastMessage(SocketMessage msg)
        {
            ShowMessage();
            var message = msg.Content.Substring(10);
            foreach (var user in Guild.Users)
            {
                if (user.Roles.Any(r => r.ToString() == "STUDENT"))
                {
                    await user.SendMessageAsync($"Dette er en broadcast melding fra {msg.Author.Username}\n" + message);
                }
            }
        }

        private static async Task SolveQuestion(SocketMessage msg)
        {
            ShowMessage();
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

            DisplayQuestionsInQueueStatus();
        }

        private static async Task ListActiveQuestions(SocketMessage msg)
        {
            ShowMessage();
            var active = 0;
            var questions = "";
            foreach (var q in ActiveQuestions)
            {
                active += q.Solved ? 0 : 1;
                if (!q.Solved) questions += $"Question ID: {q.Id}\tDate created: {q.Time}\tAssigned: {q.AssignedTo}\n";
            }

            await msg.Author.SendMessageAsync($"Antall aktive spørsmål: {active}\n" + questions);
        }

        private static async Task ReplyUnknownCommand(SocketMessage msg)
        {
            ShowMessage();
            await msg.Author.SendMessageAsync(
                "Heisann! Jeg forsto ikke helt den kommandoen... \n" +
                "Hvis du vil ha en oversikt over aktive spørsmål, send ?questions\n" +
                "Hvis du vil at jeg skal sende deg ett spørsmål, svar med ?Q\n" +
                "Hvis du vil sende en melding til alle registrerte studentder, svar med !BROADCAST [Melding]");
        }

        private static async Task ReplyWithQuestion(SocketMessage msg, Question question)
        {
            ShowMessage();
            var builder = new EmbedBuilder
            {
                Color = Color.Green,
                Description = question.Content + "\n" + question.HowToRepeat
            };
            builder.AddField("Brukernavn: ",
                    GetServer.GetUser(question.UserId).Username)
                .AddField("Spørsmål ID", question.Id)
                .AddField("Dato", question.Time)
                .AddField("Sett spørsmålet til løst ved å skrive:",
                    $"?SOLVE {question.Id}");
            Console.WriteLine($"Sent question to: {msg.Author.Username}");
            question.AssignedTo = msg.Author.Id;
            question.WriteToFile();
            await msg.Author.SendMessageAsync("", false, builder.Build());
        }

        private async Task ReplyStudent(SocketMessage msg, SocketRole userRole)
        {
            ShowMessage();
            if (msg.Content.Contains("!question"))
            {
                var q = CreateQuestion(msg);
                //var q = new Question(msg.Content, "null");
                Console.WriteLine("Made object");
                q.AddToFile();
                ActiveQuestions.Add(q);
                DisplayQuestionsInQueueStatus();
            }
            else
            {
                await msg.Author.SendMessageAsync("Heisann! Hvis du har ett spørsmål til oss, svar med " +
                                                  "!question \"[Spørsmålet ditt] | [Hvordan vi kan gjenskape problemet]\"" +
                                                  "\n __**eksempel:**__ \n!question Hvordan sender jeg kommandoer til botten | Prøver å sende en kommando, men det skjer ingen ting\n" +
                                                  "**!NB!** Husk å ha med \"|\" mellom hver del av spørsmålet du skal sende! ;)");
            }
        }

        private Question CreateQuestion(SocketMessage msg)
        {
            ShowMessage();
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
            ShowMessage();
            if (arg1.Value is null)
            {
                SendMessageBotChannel(
                    $"Message ID: {arg1.Id} Deleted from channel {arg2.Name}",
                    "Deletion",
                    "Automatic");

                Logging("Message ID: " + arg1.Id + " Deleted");
                Log(new LogMessage(LogSeverity.Info, "Delete", $"Message: {arg1.Id} deleted"));
            }
            else
            {
                SendMessageBotChannel(
                    $"Message ID: {arg1.Value.Id} Deleted from channel {arg2.Name}\n" +
                    $"Author {arg1.Value.Author}\n" +
                    $"Msg {arg1.Value.Content}",
                    "Deletion",
                    "Automatic");

                Logging("Message ID: " + arg1.Id + " Deleted content: \n" + arg1.Value.Content);
                Log(new LogMessage(LogSeverity.Info, "Delete", $"Message: {arg1.Value.Id} deleted"));
            }
            Console.WriteLine(arg1.Value.Content);

            return Task.CompletedTask;
        }

        private async Task AnnounceUserJoined(SocketGuildUser user)
        {
            ShowMessage();
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

        private static async Task AssignRole(SocketGuildUser user)
        {
            ShowMessage();
            //var user = Context.User;
            //var role = GetServer.Roles.FirstOrDefault(x => x.Name == "STUDENT"); //GET SERVER
            var role = Guild.Roles.FirstOrDefault(x => x.Name == "STUDENT"); //TESTING SERVER
            foreach (var role2 in GetServer.Roles)
            {
                Console.WriteLine(role2.Name);
            }

            await user.AddRoleAsync(role);
        }

        public static Task Log(LogMessage message)
        {
            ShowMessage();
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
            Console.WriteLine($"[{DateTime.Now,-19}] ({message.Severity}) {message.Source}: {message.Message} {message.Exception}");
            Logging($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            ShowMessage();
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            ShowMessage();
            var message = arg as SocketUserMessage;
            Logging(arg.ToString());
            if (message is null || message.Author.IsBot) return;
            if (message.Channel.Id == 553158838291791872 || message.Channel.Id == _startIT4_general)
            {
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
            
        }
        #endregion

        #region Methods
        private static void Daemon()
        {
            if (File.Exists(_daemonPath))
            {
                Log(new LogMessage(LogSeverity.Info, "Daemon Startup", "Starting Crash Handler Daemon"));
                Process.Start(_daemonPath);

            }
            else
            {
                Log(new LogMessage(LogSeverity.Error, "Daemon Startup", "Daemon Path not found"));
                Console.WriteLine("!!DAEMON PATH IS NOT VALID OR DAEMON PROGRAM NOT FOUND!!");
                Console.WriteLine("Start bot without Daemon? (y/n)");
                var input = Console.ReadLine();
                if (input != null && !(input.Contains("y") || input.Contains("Y")))
                {
                    throw new Exception("DAEMON NOT FOUND, Startup aborted by user");
                }
            }
        }

        private static void InitializeFiles()
        {
            InitFile(path, "LOGFILE GET BOT");
            InitFile(_userPath, "Brukere som har blitt registrert\n");
        }

        private void UserActionTest(SocketUser user, SocketVoiceState after)
        {
            Console.WriteLine(after.VoiceChannel.Name + " " + user.Username);
            if (after.VoiceChannel.Name == "null")
            {
                Console.WriteLine("It's null");
            }
        }

        private void PostDailyReminder(object sender, TimerAlertsEventArgs e)
        {
            Console.WriteLine("12 o' clock daily reminder");
        }

        private async void PostFridayReminder(object sender, TimerAlertsEventArgs e)
        {
            Console.WriteLine("Trying to post message");
            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithColor(Color.Blue)
                .WithDescription(
                    $"Heisann! Nå er det fredag. Husk å spille inn ukens video!\n Spill inn i OBS og send til " +
                    $"{_client.GetUser(268754579988938752).Mention}" +
                    $"/{_client.GetUser(363256000800751616).Mention}" +
                    $"/{_client.GetUser(112955646701297664).Mention}")
                .WithImageUrl(@"https://i.pinimg.com/originals/a8/ed/1e/a8ed1e3a3545b69b2aeb8512b6a55917.jpg")
                .AddField("Husk å:",
                    "Gå over hva du har lært, men også kanskje aller viktigst hva her vært " +
                    "vanskelig eller har jeg ikke fått til *ennå*! Husk at man lærer mest når man feiler!");

            //StartIt4GeneralTextChannel.SendMessageAsync("", false, builder.Build());
            await _client.GetGuild(_GET_server).GetTextChannel(538290239135940612)
                .SendMessageAsync("test", false, builder.Build());
            //BotChannel.SendMessageAsync("", false, builder.Build());
        }

        private async void TakeAttendance(object sender, TimerAlertsEventArgs e)
        {
            ShowMessage();
            await GeneralChannel.SendMessageAsync("Klokken er nå 10:00. Jeg tar oppmøte");
            var result = RegisterUsersAutomatic.Register();
            await BotChannel.SendMessageAsync($"Active users: {result.Item2.Count}");
            result.Item2.ForEach(x => Console.WriteLine(GetServer.GetUser(x).Username));
            await _client.GetUser(112955646701297664).SendFileAsync(result.Item1);
        }

        private static void Logging(string message)
        {
            ShowMessage();
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(message);
            }

        }

        public static void SendMessageBotChannel(string result, string action, string user, SocketUserMessage message = null)
        {
            ShowMessage();
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Blue)
                .WithCurrentTimestamp()
                .AddField("Action:", action)
                .AddField("User: ", user)
                .AddField("Result: ", result);
            BotChannel.SendMessageAsync("", false, builder.Build());
        }

        private static void SendError(SocketUserMessage message, IResult result)
        {
            ShowMessage();
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("ERROR")
                .AddField("Invoking message", message.Content)
                .AddField("Invoking user", message.Author)
                .WithDescription(result.ErrorReason)
                .WithCurrentTimestamp()
                .WithColor(Color.Red);
            ErrorChannel.SendMessageAsync("", false, builder.Build());
        }

        private static void ShowMessage(string message = "In method", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null)
        {

            if (StartDebugOn)
            {
                var time = DateTime.Now;
                
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"[{time.ToLocalTime()}]\t{message} at line {lineNumber}({caller})");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static async void DisplayQuestionsInQueueStatus()
        {
            var questions = ActiveQuestions.Count(x => !x.Solved);
            await _client.SetGameAsync($"Active questions: {questions}");
        }

        private void AddEventSubscriptions()
        {
            _client.Log += Log; // Adds the local Log() Event handler to the client.
            _client.UserJoined += AnnounceUserJoined; //Add event handler to client.
            _client.MessageDeleted += MessageDeleted;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += ReplyUserDmAsync;
            _client.UserLeft += HandleUserLeaveAsync;
            //_client.GuildMemberUpdated += ReportMemberUpdateAsync;
            _client.UserVoiceStateUpdated += HandleUserVoiceActionAsync;


            _alerts.RegisterUsers += TakeAttendance;
            _alerts.FridayReminder += PostFridayReminder;
            _alerts.TwelveOClock += PostDailyReminder;
        }

        private static void SetDebugMode()
        {
            var dbug = Console.ReadLine();
            if (dbug == "n" || dbug == "N")
            {
                StartDebugOn = false;
            }
            else if (dbug == "y" || dbug == "y")
            {
                StartDebugOn = true;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unable to parse command, starting in debug as default");
                Console.BackgroundColor = ConsoleColor.Black;
                StartDebugOn = true;
            }
        }

        private void InitializeClient()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            _alerts = new TimerAlerts();
        }

        private static void InitFile(string filePath, string startText)
        {
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(startText);
                }
            }
        }

        #endregion


    }


}

