using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Thickness.classi;
using Thickness.classi.gestioneGioco.GestioneCoda;
using Thickness.classi.gestionePersonale.abbonamenti;

namespace Thickness
{

    //Todo:
    /*
     * Creare tutto il setup del salvataggio file
     * 
     * Poter aggiungere un utente manualmente, 
     * Aggiungere la possibilita di cercare un utente in particolare
     * possibilita eliminare un utente o modificare (modificare solo quelli messi manualmente)
     * salvare per ogni utente manualmente iscritto un file json ("Iscrizione_(cod_f).json") e tutti i dati dentro
     * 
     * 179
     * 
     * Sistemare la pagina del piano, mettendo 30 picturebox
     * 
     * Setup grafica (Chiedere a Matteo)
     * 
     */

    public partial class Form1 : Form
    {
        ThicknessApp Thickness;
        UsersGetter usersGetter = new UsersGetter();
        GameUsers gameUsers = new GameUsers();
        private double tot = 0;

        //Gestire nel salvataggio file (Che si ricorda i prezzi)
        public int PGionaliero = 5;
        public int PMensile = 50;
        public int PAnnuale = 400;

        private TabPage pianoSave;

        public Form1()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage4)
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

                    Thickness.PayMonthlyFeeOnMonthStart(gameUsers.getSpesa());
                    Thickness.addRefund(gameUsers.getSpeseTot());

                    PGiorna.Text = PGionaliero.ToString();
                    PMens.Text = PMensile.ToString();
                    PAnno.Text = PAnnuale.ToString();
                    spesaMensile.Text = gameUsers.getSpesa().ToString() + " €";

                }
            };
            updateTimer.Start();

        }
        private User s;
        private bool Discount = false;
        private void NextOne_Click(object sender, EventArgs e)
        {

            //Sistemare la coda, in modo che ogni tot secondi, causali compare qualcuno e va in coda

            if(Manuale.Checked)
            {
                MessageBox.Show("Inserimento manuale attivato!\nImpossibile generare un nuovo utente");
                return;
            }

            s = usersGetter.GetRandomUser();
            if (gameUsers.Count() == gameUsers.getMaxUsers())
            {
                MessageBox.Show("Hai già raggiunto il numero massimo di utenti!");
                return;
            }
            

            RName.Text = s.nome;
            RSurname.Text = s.cognome;
            REmail.Text = s.email;
            RCodF.Text = s.CodF;
            RResidenza.Text = s.residenza;
            RTelefono.Text = s.numeroTelefono;
            RGender.Text = s.gender;
            RNascita.Text = s.dataNascita.ToString("dd/MM/yyyy");

            if (gameUsers.AlreadyExists(s))
            {
                discount.Visible = true;
                Discount = true;
            }

        }

        private void IscrivitiButton_Click(object sender, EventArgs e)
        {
            if (RName.Text == "")
            {
                MessageBox.Show("Devi premere \"Prossimo\" per selezionare un utente!\nOppure inserire un utente manualmente");
                return;
            }
            if (!RGiorni.Checked && !RMesi.Checked && !RAnni.Checked)
            {
                MessageBox.Show("Devi selezionare un periodo di abbonamento!");
                return;
            }

            if (RCount.SelectedIndex == -1)
            {
                MessageBox.Show("Devi selezionare un numero di abbonamenti!");
                return;
            }

            if (Manuale.Checked)
            {

                //FIxare che si puo abbonare anche se gia abbonato

                s = new User(RName.Text, RSurname.Text, REmail.Text, RCodF.Text,
                    RTelefono.Text, RGender.Text, DateTime.Parse(RNascita.Text), RResidenza.Text);
                saveManualUser(s);
            }

            DateTime fine = DateTime.Now;
            int count = RCount.SelectedIndex + 1;
            if (RGiorni.Checked)
            {
                fine = fine.AddDays(count);
            }
            else if (RMesi.Checked)
            {
                fine = fine.AddMonths(count);
            }
            else if (RAnni.Checked)
            {
                fine = fine.AddYears(count);
            }

            if (gameUsers.isAlreadyAbb(s))
            {
                MessageBox.Show("L'utente é gia abbonato!");
                return;
            }

            if(Discount)
            {
                Discount = false;
                tot = tot * 0.8;
            }

            

            Thickness.addCash(tot);

            gameUsers.addabbon(s, DateTime.Now, fine, (int)tot);
            MessageBox.Show(DateTime.Now + " -- " + fine);



        }

        private void saveManualUser(User e)
        {
            if(checkIfPresent(e))
            {
                return;
            }
            string path = @"../../script/UtenzeManuali/Iscrizioni_" + e.CodF + ".json";
            try
            {
                if (!Directory.Exists(@"../../script/UtenzeManuali"))
                {
                    Directory.CreateDirectory(@"../../script/UtenzeManuali");
                }

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(e, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(path, json);
                MessageBox.Show("Utente salvato con successo in: " + path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante il salvataggio dell'utente: " + ex.Message);
            }
        }

        private bool checkIfPresent(User e)
        {
            string path = @"../../script/UtenzeManuali/Iscrizioni_" + e.CodF + ".json";
            if (File.Exists(path))
            {
                return true;
            }
            return false;
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

        private void PGiorna_TextChanged(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                int new_price = 0;
                try
                {
                    new_price = int.Parse(PGiorna.Text);
                }
                catch (FormatException ex)
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
                MessageBox.Show("Il prezzo è stato cambiato a " + PGionaliero.ToString() + " € al giorno");
            }

        }

        private void PMens_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
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
                MessageBox.Show("Il prezzo è stato cambiato a " + PMensile.ToString() + " € al mese");
            }
        }

        private void PAnno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evita il beep di sistema quando si preme Invio  
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
                MessageBox.Show("Il prezzo è stato cambiato a " + PAnnuale.ToString() + " € per un anno");
            }
        }

        private void CompraMacchinario_Click(object sender, EventArgs e)
        {
            if (Thickness.getCash() < 10000)
            {
                MessageBox.Show("Non hai abbastanza soldi per comprare un macchinario!");
                return;
            }
            if (gameUsers.addMacchinario())
            {
                Thickness.removeCash(10000);
                MessageBox.Show("Hai comprato un macchinario!");
                //Aggiorna i macchinari
                Macchinari.Text = gameUsers.getMacchinari().ToString();
                return;
            }
            MessageBox.Show("Hai già raggiunto il numero massimo di macchinari!");

        }

        private void Hack_Click(object sender, EventArgs e)
        {
            Thickness.addCash(1000000);
        }

        private void CompraPiano_Click(object sender, EventArgs e)
        {

            //TO-DO
            /*
             * Sistemare il setup dei piani
             */
            if (Thickness.getCash() < 100000)
            {
                MessageBox.Show("Non hai abbastanza soldi per comprare un'altro piano!");
                return;
            }
            if (gameUsers.addPiano())
            {
                Thickness.removeCash(100000);
                MessageBox.Show("Hai comprato un altro piano!");
                //Aggiorna i piani
                TabPage pianoSaveToUse = pianoSave;
                
                tabControl1.TabPages.Add(pianoSaveToUse);
                pianoSaveToUse = tabControl1.TabPages[tabControl1.TabCount - 1];
                pianoSaveToUse.Text = "Piano " + (tabControl1.TabCount - 3).ToString();
                pianoSaveToUse.Name = "Piano" + (tabControl1.TabCount - 3).ToString();


                Piani.Text = gameUsers.getPiani().ToString();
                return;
            }
            MessageBox.Show("Hai già raggiunto il numero massimo di Piani!");
        }

        private DateTime lastAdd1DayClick = DateTime.MinValue;

        private void add1Day_Click(object sender, EventArgs e)
        {
            if ((DateTime.Now - lastAdd1DayClick).TotalSeconds < 20)
            {
                MessageBox.Show("Devi aspettare 20 secondi prima di poter aggiungere un altro giorno!");
                return;
            }

            Thickness.addTime(new TimeSpan(1, 0, 0, 0));
            lastAdd1DayClick = DateTime.Now;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pianoSave = tabControl1.TabPages[3];
        }

        private void OneMSkip_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Lo skip di 1m costa 20k\n Sei sicuro di voler continuare?", "Conferma", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                if(Thickness.getCash() < 20000)
                {
                    MessageBox.Show("Non hai abbastanza soldi per fare lo skip!");
                    return;
                }
                Thickness.removeCash(20000);
                Thickness.addTime(new TimeSpan(30, 0, 0, 0));
                Thickness.PayMonthlyFeeOnMonthStart(gameUsers.getSpesa());
                MessageBox.Show("Hai saltato 1 mese!");
            }
            else
            {
                MessageBox.Show("Operazione annullata!");
            }
        }

        private void Manuale_CheckedChanged(object sender, EventArgs e)
        {
            if(Manuale.Checked)
            {
                RName.Text = "";
                RName.Enabled = true;
                RSurname.Text = "";
                RSurname.Enabled = true;
                REmail.Text = "";
                REmail.Enabled = true;
                RCodF.Text = "";
                RCodF.Enabled = true;
                RResidenza.Text = "";
                RResidenza.Enabled = true;
                RTelefono.Text = "";
                RTelefono.Enabled = true;
                RGender.Text = "";
                RGender.Enabled = true;
                RNascita.Text = "";
                RNascita.Enabled = true;

            }else
            {
                RName.Text = "";
                RName.Enabled = false;
                RSurname.Text = "";
                RSurname.Enabled = false;
                REmail.Text = "";
                REmail.Enabled = false;
                RCodF.Text = "";
                RCodF.Enabled = false;
                RResidenza.Text = "";
                RResidenza.Enabled = false;
                RTelefono.Text = "";
                RTelefono.Enabled = false;
                RGender.Text = "";
                RGender.Enabled = false;
                RNascita.Text = "";
                RNascita.Enabled = false;
            }
            
        }
    }
}
