using System;
using System.Timers;

namespace DiscordBot
{
    public class NoonTimer
    {
        private Timer _timer;
        public event EventHandler<NoonAlertEventArgs> NoonAlert;
        private int _lastDateAlerted;

        public NoonTimer()
        {
            _timer = new Timer(30000);
            _timer.Elapsed += timerElapsed;
            _timer.Start();


        }

        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if (now.Hour == 12 && now.Date.Day != _lastDateAlerted)
            {
                if (NoonAlert != null)
                {
                    NoonAlert(this, new NoonAlertEventArgs(){Something = 5});
                }
                _lastDateAlerted = now.Date.Day;
            }
        }

        private void AnnounceFridayReminder(object state)
        {
            //GeneralChannel.SendMessageAsync("Klokken er nå 12:00");
        }
    }
}
