using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        

        public void addRefund(long spent)
        {
            DateTime now = this.time.TimeUpdated();
            DateTime lastUpdate = this.time.TimeStarted();

            if (flag)
            {
                if (now.Day != 1)
                {
                    flag = false;
                }
                return;
            }

            // Check if it's the end of the month  
            if (now.Day == 1 && !flag)
            {
                this.removeCash(spent); // Deduct monthly fee  
                flag = true;
                MessageBox.Show("Pagamento mensile eseguito con successo a fine mese: " + spent + "€");
            }
        }

        /*public void PayMonthlyFeeOnMonthStart(long rentAmount)
        {
            DateTime now = this.time.TimeUpdated();
            DateTime lastUpdate = this.time.TimeStarted();

            if (flag)
            {
                if (now.Day != DateTime.DaysInMonth(lastUpdate.Year, lastUpdate.Month))
                {
                    MessageBox.Show("Qui");
                    flag = false;
                }
                return;
            }


            // Check if it's the last day of the month  
            if (now.Day == DateTime.DaysInMonth(now.Year, now.Month) && !flag)
            {
                this.removeCash(rentAmount); // Deduct rent amount  
                flag = true;
                MessageBox.Show("Affitto pagato con successo: " + rentAmount + "€");
            }
        }*/

        public void PayMonthlyFeeOnMonthStart(long rentAmount, bool doit)
        {
            this.removeCash(rentAmount); 
            MessageBox.Show("Affitto pagato con successo: " + rentAmount + "€");
            flag = false;
        }

        bool payed = false;

        public void PayMonthlyFeeOnMonthStart(long rentAmount)
        {

            DateTime now = this.time.TimeUpdated();
            DateTime lastUpdate = this.time.TimeStarted();
            if (payed)
            {
                if(now.Day != 1)
                {
                    payed = false;
                }
                return;
            }

            

            // Check if it's the first day of the month  
            if (now.Day == 1 && !payed)
            {
                this.removeCash(rentAmount); // Deduct rent amount  
                payed = true;
                MessageBox.Show("Affitto pagato con successo: " + rentAmount + "€");
            }
        }



    }
}
