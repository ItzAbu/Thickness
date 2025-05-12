using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Thickness.classi.gestioneCash;
using Thickness.classi.gestioneTempo;

namespace Thickness.classi
{
    internal class ThicknessApp
    {
        GameCash cash;
        GameTime time; // Example daily expense amount  
        

        public ThicknessApp(DateTime n)
        {
            this.cash = new GameCash();
            this.time = new GameTime(n);
            
        }

        public void start()
        {
            
            this.time.StartTimer();
            this.time.AddTimeElapsedHandler(OnDailyExpenseTimerElapsed);

        }

        public void StartDailyExpense()
        {
            this.time.AddTimeElapsedHandler(OnDailyExpenseTimerElapsed);
        }

        public void addEvent(ElapsedEventHandler eventHandler)
        {
            this.time.AddTimeElapsedHandler(eventHandler);
        }

        private void OnDailyExpenseTimerElapsed(object sender, EventArgs e)
        {
            int spesa = 0;
            this.removeCash(spesa);
        }

        public bool addCash(double amount)
        {
            return this.cash.addCash(amount);
        }

        public bool removeCash(double amount)
        {
            return this.cash.remCash(amount);
        }

        public double getCash()
        {
            return this.cash.getCash();
        }


        public void stopTimer()
        {
            this.time.StopTimer();
        }

        public void resetTime()
        {
            this.time.ResetTime();
        }

        public bool addTime(TimeSpan t)
        {
            return this.time.AddTime(t);
        }

        public bool isRunning()
        {
            return this.time != null && this.time.IsRunning();
        }

        public DateTime getGameTime()
        {
            return time.TimeUpdated();
        }


    }
}
