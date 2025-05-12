using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Thickness.classi.gestioneTempo;
using Timer = System.Timers.Timer;

namespace Thickness.classi.gestioneTempo
{
    internal class GameTime
    {
        private DateTime startTime;
        private Timer timer;
        private DateTime currentTime;

        public GameTime(DateTime currentTime)
        {
            this.startTime = currentTime;
            this.timer = new Timer(1000); // 1000 ms = 1 secondo  
            this.timer.Elapsed += OnTimerElapsed;
            this.currentTime = currentTime;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.currentTime = this.currentTime.AddHours(1);
        }

        public void StartTimer()
        {
            if (this.timer != null && !this.timer.Enabled)
            {
                this.timer.Start();
            }
        }

        public TimeSpan GetElapsedTime()
        {
            return this.currentTime - this.startTime;
        }

        public void StopTimer()
        {
            if (this.timer != null && this.timer.Enabled)
            {
                this.timer.Stop();
            }
        }

        public void ResetTime()
        {
            this.startTime = DateTime.Now;
            this.currentTime = this.startTime;
        }

        public bool AddTime(TimeSpan timeToAdd)
        {
            if (timeToAdd.TotalSeconds < 0)
            {
                return false; // Invalid time to add  
            }
            this.currentTime = this.currentTime.Add(timeToAdd);
            return true;
        }

        public bool IsRunning()
        {
            return this.timer != null && this.timer.Enabled;
        }

        public void AddTimeElapsedHandler(ElapsedEventHandler handler)
        {
            if (this.timer != null)
            {
                this.timer.Elapsed += handler;
            }
        }

        public void RemoveTimeElapsedHandler(ElapsedEventHandler handler)
        {
            if (this.timer != null)
            {
                this.timer.Elapsed -= handler;
            }
        }

        public DateTime TimeUpdated()
        {
            return this.currentTime;
        }

        public DateTime TimeStarted()
        {
            return this.startTime;
        }
    }
}


