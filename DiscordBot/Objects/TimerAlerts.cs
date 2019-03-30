using System;
using System.Timers;

namespace DiscordBot.Objects
{
    public class TimerAlerts
    {
        public event EventHandler<TimerAlertsEventArgs> RegisterUsers;
        public event EventHandler<TimerAlertsEventArgs> FridayReminder;
        public event EventHandler<TimerAlertsEventArgs> TwelveOClock;
        private int _lastDateAlertedRegisterUsers;
        private int _lastDateAlertedInfo;
        private int _fridayAlerted;
        /*
         * Add local stored variable that can be set if the alert already has been sent, to mitigate spam on repeated startups
         */
        public TimerAlerts()
        {
            var timerDaily = new Timer(30000);
            var timerDailyTwelve = new Timer(30000);
            var timerFriday = new Timer(3000);

            timerDaily.Elapsed += TimerDailyElapsed;
            timerDaily.Start();

            timerFriday.Elapsed += TimerFridayElapsed;
            timerFriday.Start();

            timerDailyTwelve.Elapsed += TimerTwelveElapsed;
            timerDailyTwelve.Start();
        }

        private void TimerTwelveElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if (now.Hour >= 12 && now.Hour < 13 && now.Date.Day != _lastDateAlertedInfo)
            {
                TwelveOClock?.Invoke(this, new TimerAlertsEventArgs());

                _lastDateAlertedInfo = now.Date.Day;
            }
        }

        private void TimerFridayElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if (now.Hour >= 10 && now.Hour < 11 && now.DayOfWeek == DayOfWeek.Friday && now.Day != _fridayAlerted)
            {
                FridayReminder?.Invoke(this, new TimerAlertsEventArgs());

                _fridayAlerted = now.Date.Day;
            }
        }

        private void TimerDailyElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if (now.Hour >= 10 && now.Hour < 11 && now.DayOfWeek != DayOfWeek.Saturday && now.DayOfWeek != DayOfWeek.Sunday && now.Date.Day != _lastDateAlertedRegisterUsers)
            {
                RegisterUsers?.Invoke(this, new TimerAlertsEventArgs());

                _lastDateAlertedRegisterUsers = now.Date.Day;
            }
        }
    }
}