using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thickness.classi;
using Thickness.classi.gestioneGioco.GestioneCoda;
using Thickness.classi.gestionePersonale.abbonamenti;
using Thickness.classi.gestioneTempo;

namespace Thickness
{
    public partial class Form1 : Form
    {
        ThicknessApp Thickness;
        UsersGetter usersGetter = new UsersGetter();
        GameUsers gameUsers = new GameUsers();
        private double tot = 0;

        public int PGionaliero = 5;
        public int PMensile = 50;
        public int PAnnuale = 400;

        public Form1()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab == tabPage4)
            {
                abbBoxSetup();
            }
            if (tabControl1.SelectedTab != tabPage2)
            {
                return;
            }
            if (Thickness == null)
            {
                tabControl1.SelectedTab = tabPage1;
                MessageBox.Show("Devi premere \"START\" per cominciare il gioco!");
                return;
            }
        }

        private void MainStart_Click(object sender, EventArgs e)
        {
            if (MainNickName.Text == "")
            {
                MessageBox.Show("Devi inserire un nickname!");
                return;
            }
            if (MainNickName.Text.Length > 10)
            {
                MessageBox.Show("Il nickname non può superare i 10 caratteri!");
                return;
            }

            if (MainNickName.Text.Length < 3)
            {
                MessageBox.Show("Il nickname deve essere lungo almeno 3 caratteri!");
                return;
            }
            Thickness = new ThicknessApp(DateTime.Now);
            Thickness.start();
            MessageBox.Show("Benvenuto " + MainNickName.Text + "!");
            MainStart.Enabled = false;
            StartUpdatingMainTime();
            MainNickName.Enabled = false;
            loginName.Text = MainNickName.Text;
        }

        private void StartUpdatingMainTime()
        {
            Timer updateTimer = new Timer();
            updateTimer.Interval = 1000; // 10 secondi  
            updateTimer.Tick += (sender, e) =>
            {
                if (Thickness != null)
                {
                    
                    MainTime.Text = Thickness.getGameTime().ToString();
                    MainMoney.Text = Thickness.getCash().ToString("F2") + " €";
                    MainAbbonati.Text = gameUsers.abbAttivi(Thickness.getGameTime()).Count().ToString();
                    MainUtentiTotali.Text = gameUsers.Count().ToString();
                }
            };
            updateTimer.Start();

        }
        private User s;
        private void NextOne_Click(object sender, EventArgs e)
        {
            
            //Sistemare la coda, in modo che ogni tot secondi, causali compare qualcuno e va in coda

            s = usersGetter.GetRandomUser();
            if(gameUsers.Count() == 200)
            {
                MessageBox.Show("Hai già raggiunto il numero massimo di utenti!");
                return;
            }
            while (gameUsers.AlreadyExists(s))
            {
                s = usersGetter.GetRandomUser();
            }

            RName.Text = s.nome;
            RSurname.Text = s.cognome;
            REmail.Text = s.email;
            RCodF.Text = s.CodF;
            RResidenza.Text = s.residenza;
            RTelefono.Text = s.numeroTelefono;
            RGender.Text = s.gender;
            RNascita.Text = s.dataNascita.ToString("dd/MM/yyyy");

        }

        private void IscrivitiButton_Click(object sender, EventArgs e)
        {
            if(RName.Text == "")
            {
                MessageBox.Show("Devi premere \"Prossimo\" per selezionare un utente!");
                return;
            }
            if(!RGiorni.Checked && !RMesi.Checked && !RAnni.Checked)
            {
                MessageBox.Show("Devi selezionare un periodo di abbonamento!");
                return;
            }

            if(RCount.SelectedIndex == -1)
            {
                MessageBox.Show("Devi selezionare un numero di abbonamenti!");
                return;
            }
            DateTime fine = DateTime.Now;
            int count = RCount.SelectedIndex + 1;
            if(RGiorni.Checked)
            {
                fine = fine.AddDays(count);
            }
            else if(RMesi.Checked)
            {
                fine = fine.AddMonths(count);
            }else if(RAnni.Checked)
            {
                fine = fine.AddYears(count);
            }

            if (gameUsers.isAlreadyAbb(s))
            {
                MessageBox.Show("L'utente é gia abbonato!");
                return;
            }

            Thickness.addCash(tot);
            
            gameUsers.addabbon(s, DateTime.Now, fine, (int)tot);
            MessageBox.Show(DateTime.Now + " -- " + fine);



        }


        //Gestisce il prezzo in base ai giorni
        private void RGiorni_CheckedChanged(object sender, EventArgs e)
        {
            tot = 0;

            if (RGiorni.Checked)
            {
                tot = PGionaliero;
            }
            else if (RMesi.Checked)
            {
                tot = PMensile;
            }
            else if (RAnni.Checked)
            {
                tot = PAnnuale;
            }

            tot *= (RCount.SelectedIndex + 1);

            RTot.Text = tot.ToString();
        }

        //Gestisce il prezzo in base al tipo
        private void RCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!RGiorni.Checked && !RMesi.Checked && !RAnni.Checked)
            {
                return;
            }

            

            if (RGiorni.Checked)
            {
                tot = PGionaliero;
            }
            else if (RMesi.Checked)
            {
                tot = PMensile;
            }
            else if (RAnni.Checked)
            {
                tot = PAnnuale;
            }

            tot *= (RCount.SelectedIndex + 1);

            RTot.Text = tot.ToString();
        }

        //Gestione abbonati pagine Ufficio

        private void abbBoxSetup()
        {
            List<Abbonamento> abbAttivi = gameUsers.getActiveAbb();
            List<Abbonamento> abbFiniti = gameUsers.getEndedAbb();
            OfficeAbbBox.Clear();
            OfficeAbbBox.View = View.Details; // Imposta la visualizzazione a Dettagli per mostrare le colonne  
            OfficeAbbBox.Columns.Clear(); // Rimuove eventuali colonne esistenti  

            // Aggiunge le colonne al ListView  
            OfficeAbbBox.Columns.Add("Nome", 100);
            OfficeAbbBox.Columns.Add("Cognome", 100);
            OfficeAbbBox.Columns.Add("Data Inizio", 100);
            OfficeAbbBox.Columns.Add("Fine", 100);
            OfficeAbbBox.Columns.Add("Prezzo", 100);
            OfficeAbbBox.Columns.Add("Attivo", 100);

            // Aggiunge gli abbonamenti attivi  
            foreach (var u in abbAttivi)
            {
                ListViewItem item = new ListViewItem(u.user.nome);
                item.SubItems.Add(u.user.cognome);
                item.SubItems.Add(u.inizio.ToString("dd/MM/yyyy"));
                item.SubItems.Add(u.fine.ToString("dd/MM/yyyy"));
                item.SubItems.Add(u.prezzo.ToString("F2") + " €");
                item.SubItems.Add(DateTime.Now <= u.fine ? "Attivo" : "Non Attivo");
                OfficeAbbBox.Items.Add(item);
            }

            // Aggiunge gli abbonamenti terminati  
            foreach (var u in abbFiniti)
            {
                ListViewItem item = new ListViewItem(u.user.nome);
                item.SubItems.Add(u.user.cognome);
                item.SubItems.Add(u.inizio.ToString("dd/MM/yyyy"));
                item.SubItems.Add(u.fine.ToString("dd/MM/yyyy"));
                item.SubItems.Add(u.prezzo.ToString("F2") + " €");
                item.SubItems.Add(DateTime.Now <= u.fine ? "Attivo" : "Non Attivo");
                OfficeAbbBox.Items.Add(item);
            }
        }

        private void PGiorna_TextChanged(object sender, EventArgs e)
        {
            int new_price = 0;
            try
            {
                new_price = int.Parse(PGiorna.Text);
            }catch(FormatException ex)
            {
                MessageBox.Show("Devi inserire un numero!");
                PGiorna.Text = PGionaliero.ToString();
                return;
            }
            if (new_price < 0)
            {
                MessageBox.Show("Il prezzo non può essere negativo!");
                PGiorna.Text = PGionaliero.ToString();
                return;
            }

            if (new_price == 0)
            {
                MessageBox.Show("Il prezzo non può essere 0!");
                PGiorna.Text = PGionaliero.ToString();
                return;
            }

            PGionaliero = new_price;


        }

        private void PMens_TextChanged(object sender, EventArgs e)
        {

            int new_price = 0;
            try
            {
                new_price = int.Parse(PMens.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Devi inserire un numero!");
                PMens.Text = PMensile.ToString();
                return;
            }
            if (new_price < 0)
            {
                MessageBox.Show("Il prezzo non può essere negativo!");
                PMens.Text = PMensile.ToString();
                return;
            }

            if (new_price == 0)
            {
                MessageBox.Show("Il prezzo non può essere 0!");
                PMens.Text = PMensile.ToString();
                return;
            }

            PMensile = new_price;

        }

        private void PAnno_TextChanged(object sender, EventArgs e)
        {
            int new_price = 0;
            try
            {
                new_price = int.Parse(PAnno.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Devi inserire un numero!");
                PAnno.Text = PAnnuale.ToString();
                return;
            }
            if (new_price < 0)
            {
                MessageBox.Show("Il prezzo non può essere negativo!");
                PAnno.Text = PAnnuale.ToString();
                return;
            }

            if (new_price == 0)
            {
                MessageBox.Show("Il prezzo non può essere 0!");
                PAnno.Text = PAnnuale.ToString();
                return;
            }

            PAnnuale = new_price;
        }
    }
}
