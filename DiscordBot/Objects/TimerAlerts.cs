using System;
using System.Timers;

namespace DiscordBot.Objects
{
    public class TimerAlerts
    {
        private Timer _timerDaily;
        private Timer _timerFriday;
        private Timer _timerDailyTwelve;
        public event EventHandler<TimerAlertsEventArgs> RegisterUsers;
        public event EventHandler<TimerAlertsEventArgs> FridayReminder;
        public event EventHandler<TimerAlertsEventArgs> TwelveOClock;
        private int _lastDateAlertedRegisterUsers;
        private int _lastDateAlertedInfo;
        private int _FridayAlerted;
        /*
         * Add local stored variable that can be set if the alert already has been sent, to mitigate spam on repeated startups
         */
        public TimerAlerts()
        {
            _timerDaily = new Timer(30000);
            _timerDailyTwelve = new Timer(30000);
            _timerFriday = new Timer(3000);

            _timerDaily.Elapsed += TimerDailyElapsed;
            _timerDaily.Start();

            _timerFriday.Elapsed += TimerFridayElapsed;
            _timerFriday.Start();

            _timerDailyTwelve.Elapsed += TimerTwelveElapsed;
            _timerDailyTwelve.Start();
        }

        private void TimerTwelveElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if (now.Hour >= 10 && now.Date.Day != _lastDateAlertedInfo)
            {
                if (TwelveOClock != null)
                {
                    TwelveOClock(this, new TimerAlertsEventArgs());
                }

                _lastDateAlertedInfo = now.Date.Day;
            }
        }

        private void TimerFridayElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if (now.Hour >= 11 && now.DayOfWeek == DayOfWeek.Friday && now.Day != _FridayAlerted)
            {
                if (FridayReminder != null)
                {
                    FridayReminder(this, new TimerAlertsEventArgs());
                }

                _FridayAlerted = now.Date.Day;
            }
        }

        private void TimerDailyElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if (now.Hour >= 10 && now.Date.Day != _lastDateAlertedRegisterUsers)
            {
                if (RegisterUsers != null)
                {
                    RegisterUsers(this, new TimerAlertsEventArgs());
                }

                _lastDateAlertedRegisterUsers = now.Date.Day;
            }
        }
    }
}