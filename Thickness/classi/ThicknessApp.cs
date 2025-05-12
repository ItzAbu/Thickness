using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Thickness.classi.gestioneCash;
using Thickness.classi.gestioneTempo;

namespace Thickness.classi
{
    internal class ThicknessApp
    {
        GameCash cash;
        GameTime time;  
        

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

        bool flag = false;
        public void PayMonthlyFeeOnMonthStart(long spent)
        {
            DateTime now = this.time.TimeStarted();
            DateTime lastUpdate = this.time.TimeUpdated();

            if (flag)
            {
                if (now.Day != lastUpdate.Day)
                {
                    flag = false;
                }
                return;
            }

            // Check if it's a new month  
            if (now.Month == lastUpdate.Month + 1 && now.Day == lastUpdate.Day)
            {
                this.removeCash(spent); // Example monthly fee amount multiplied by months passed  
                flag = true;
                MessageBox.Show("Pagamento mensile eseguito con successo: " + spent + "€");
            }
        }

        public void addRefund(long spent)
        {
            DateTime now = this.time.TimeUpdated();
            DateTime lastUpdate = this.time.TimeStarted();

            if (flag)
            {
                if (now.Day != lastUpdate.Day)
                {
                    flag = false;
                }
                return;
            }

            // Check if it's the end of the month  
            if (now.Month == lastUpdate.Month && now.Day == DateTime.DaysInMonth(now.Year, now.Month))
            {
                this.removeCash(spent); // Deduct monthly fee  
                flag = true;
                MessageBox.Show("Pagamento mensile eseguito con successo a fine mese: " + spent + "€");
            }
        }
        



    }
}
