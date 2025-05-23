using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Thickness.classi;
using Thickness.classi.gestioneGioco.GestioneCoda;
using Thickness.classi.gestionePersonale.abbonamenti;

namespace Thickness
{

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

        private Queue<Image> coda = new Queue<Image>();

        public Form1()
        {
            InitializeComponent();

            //24 491


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

        private void startCoda()
        {
            string path = @"../../Immagini/personaggi";
            string[] files = Directory.GetFiles(path, "*.png");
            foreach (string file in files)
            {
                Image image = Image.FromFile(file);
                coda.Enqueue(image);
            }

        }

        private void refillCoda()
        {
            string path = @"../../Immagini/personaggi";
            string[] files = Directory.GetFiles(path, "*.png");
            while (coda.Count < 20)
            {
                Random random = new Random();
                int index = random.Next(files.Length);
                string file = files[index];
                Image image = Image.FromFile(file);
                coda.Enqueue(image);
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


            tabControl1.SelectedTab = tabPage2;



            waitLine2.Parent = SfondoReception;
            waitLine2.BackColor = Color.Transparent;

            waitLine1.Parent = SfondoReception;
            waitLine1.BackColor = Color.Transparent;


        }

        List<PictureBox> macchinari = new List<PictureBox>();

        private void Form1_Load(object sender, EventArgs e)
        {
            pianoSave = tabControl1.TabPages[3];

            // Configura il pulsante per evitare che diventi bianco  
            NextOne.FlatStyle = FlatStyle.Flat;
            NextOne.FlatAppearance.BorderSize = 0;
            NextOne.BackColor = Color.Transparent;
            NextOne.Parent = SfondoReception;
            NextOne.FlatAppearance.MouseOverBackColor = Color.Transparent; // Imposta il colore di sfondo quando il mouse è sopra  
            NextOne.FlatAppearance.MouseDownBackColor = Color.Transparent; // Imposta il colore di sfondo quando il pulsante è premuto  

            // Aggiungi un gestore per l'evento Paint  
            NextOne.Paint += (s, args) =>
            {
                // Disegna il testo del pulsante manualmente  
                TextRenderer.DrawText(
                    args.Graphics,
                    NextOne.Text,
                    NextOne.Font,
                    NextOne.ClientRectangle,
                    NextOne.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
            };

            var list = tabControl1.TabPages;

            SaveMacchinariPerPiano();

            int macchinari = gameUsers.getMacchinari();
            Macchinari.Text = macchinari.ToString();

            load_Macchinari(macchinari);


            MainStart.Parent = sfondoStart;
            MainStart.BackColor = Color.Transparent;

            phoneOpener.Parent = telefono;
            phoneOpener.BackColor = Color.Transparent;

            startCoda();

        }

        private void SaveMacchinariPerPiano()
        {

            List<PictureBox> allMacchinari = new List<PictureBox>();

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Name.StartsWith("Piano"))
                {
                    PictureBox sfondo = null;

                    foreach (Control control2 in tab.Controls)
                    {
                        if (control2 is PictureBox pictureBox)
                        {
                            if (pictureBox.Tag == "sfondo")
                            {
                                sfondo = pictureBox;
                                continue;
                            }
                            allMacchinari.Add(pictureBox);
                            pictureBox.BackColor = Color.Transparent;



                        }
                    }

                    foreach (var a in allMacchinari)
                    {
                        if (a.Tag == "1")
                        {
                            a.Parent = sfondo;
                            a.BackColor = Color.Transparent;
                            a.Visible = false;
                        }
                    }
                }
            }

            macchinari = allMacchinari;



        }

        private void load_Macchinari(int n)
        {
            closeall();
            for (int i = 0; i < n; i++)
            {
                macchinari[i].Visible = true;
            }
        }

        private void closeall()
        {
            foreach (var m in macchinari)
            {
                m.Visible = false;
            }
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


            if (Manuale.Checked)
            {
                MessageBox.Show("Inserimento manuale attivato!\nImpossibile generare un nuovo utente");
                return;
            }

            s = usersGetter.GetRandomUser();
            if (gameUsers.Count() == gameUsers.getMacchinari()*5)
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

            waitLine1.Image = waitLine2.Image;
            waitLine2.Image = coda.Dequeue();

            refillCoda();

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

            

            if (Discount)
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
            if (checkIfPresent(e))
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

                load_Macchinari(gameUsers.getMacchinari());

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
            if (Thickness.getCash() < 100000)
            {
                MessageBox.Show("Non hai abbastanza soldi per comprare un altro piano!");
                return;
            }
            if (gameUsers.addPiano())
            {
                Thickness.removeCash(100000);
                MessageBox.Show("Hai comprato un altro piano!");

                // Duplica il contenuto di Piano1  
                TabPage newTabPage = new TabPage
                {
                    Text = "Piano " + (tabControl1.TabCount - 2).ToString(),
                    Name = "Piano" + (tabControl1.TabCount - 2).ToString()
                };


                PictureBox sfondo = null;
                foreach (Control control in Piano1.Controls)
                {
                    Control newControl = null;

                    if (control is PictureBox originalPictureBox)
                    {
                        newControl = new PictureBox
                        {
                            Size = originalPictureBox.Size,
                            Location = originalPictureBox.Location,
                            BackColor = originalPictureBox.BackColor,
                            Image = originalPictureBox.Image,
                            SizeMode = originalPictureBox.SizeMode,
                            Parent = originalPictureBox.Parent,
                            Tag = originalPictureBox.Tag,
                            Visible = true
                        };

                        


                    }


                    newTabPage.Controls.Add(newControl);
                    sfondo = (PictureBox)newControl;
                }

                foreach (PictureBox k in sfondoPiano.Controls)
                {
                    PictureBox newControl = null;
                    newControl = new PictureBox
                    {
                        Size = k.Size,
                        Location = k.Location,
                        BackColor = Color.Transparent,
                        Image = k.Image,
                        SizeMode = k.SizeMode,
                        Parent = sfondo,
                        Tag = k.Tag,
                        Visible = false
                    };

                    newTabPage.Controls.Add(newControl);


                    PictureBox box = (PictureBox)newControl;

                    if (box == sfondo)
                    {
                        continue;
                    }

                    box.BringToFront();
                    box.Parent = sfondo;
                    box.BackColor = Color.Transparent;
                    box.Visible = false;
                    macchinari.Add(box);
                }




                tabControl1.TabPages.Add(newTabPage);
                Piani.Text = gameUsers.getPiani().ToString();
                return;
            }
            MessageBox.Show("Hai già raggiunto il numero massimo di Piani!");
        }
        private DateTime lastAdd1DayClick = DateTime.MinValue;

        private void add1Day_Click(object sender, EventArgs e)
        {
            if ((DateTime.Now - lastAdd1DayClick).TotalSeconds < 2)
            {
                MessageBox.Show("Devi aspettare 20 secondi prima di poter aggiungere un altro giorno!");
                return;
            }

            Thickness.addTime(new TimeSpan(1, 0, 0, 0));
            lastAdd1DayClick = DateTime.Now;
            reload();
        }



        private void OneMSkip_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Lo skip di 1m costa 20k\n Sei sicuro di voler continuare?", "Conferma", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                if (Thickness.getCash() < 20000)
                {
                    MessageBox.Show("Non hai abbastanza soldi per fare lo skip!");
                    return;
                }
                Thickness.removeCash(20000);
                Thickness.addTime(new TimeSpan(30, 0, 0, 0));
                Thickness.PayMonthlyFeeOnMonthStart(gameUsers.getSpesa(), true);
                Thickness.addRefund(gameUsers.getSpeseTot());
                MessageBox.Show("Hai saltato 1 mese!");
                reload();
            }
            else
            {
                MessageBox.Show("Operazione annullata!");
            }
        }

        private Image save, save2;

        private void Manuale_CheckedChanged(object sender, EventArgs e)
        {
            if (Manuale.Checked)
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

                save = waitLine1.Image;
                save2 = waitLine2.Image;
                waitLine1.Image = null;
                waitLine2.Image = null;

            }
            else
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
                waitLine1.Image = save;
                waitLine2.Image = save2;
            }

        }

        private bool isPanelExpanded = false;

        private void TogglePanel1()
        {
            int expandedHeight = 491 - 100;
            int collapsedHeight = 44;
            int step = 10;
            Timer animationTimer = new Timer();
            animationTimer.Interval = 10;

            animationTimer.Tick += (sender, e) =>
            {
                if (isPanelExpanded)
                {
                    if (panel1.Height > collapsedHeight)
                    {
                        panel1.Height -= step;
                        panel1.Top += step;
                    }
                    else
                    {
                        panel1.Height = collapsedHeight;
                        animationTimer.Stop();
                        isPanelExpanded = false;
                    }
                }
                else
                {
                    if (panel1.Height < expandedHeight)
                    {
                        panel1.Height += step;
                        panel1.Top -= step;
                    }
                    else
                    {
                        panel1.Height = expandedHeight;
                        animationTimer.Stop();
                        isPanelExpanded = true;
                    }
                }
            };

            animationTimer.Start();
        }

        private void waitLine1_Click(object sender, EventArgs e)
        {

        }

        private void Reload_Click(object sender, EventArgs e)
        {
            reload();
            // Ricarica i dati aggiornati per l'utente selezionato  
            foreach (ListViewItem item in OfficeAbbBox.Items)
            {
                string nome = item.SubItems[0].Text;
                string cognome = item.SubItems[1].Text;

                User user = gameUsers.findUser(nome, cognome);

                if (user != null)
                {
                    item.SubItems[5].Text = gameUsers.isAlreadyAbb(user) ? "Attivo" : "Non Attivo";
                }
            }

            // Aggiorna i dettagli dell'utente selezionato  
            if (OfficeAbbBox.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = OfficeAbbBox.SelectedItems[0];
                string nome = selectedItem.SubItems[0].Text;
                string cognome = selectedItem.SubItems[1].Text;

                User selectedUser = gameUsers.findUser(nome, cognome);
                if (selectedUser != null)
                {
                    UName.Text = selectedUser.nome;
                    USurname.Text = selectedUser.cognome;
                    UEmail.Text = selectedUser.email;
                    UCodF.Text = selectedUser.CodF;
                    UTelefono.Text = selectedUser.numeroTelefono;
                    UGenere.Text = selectedUser.gender;
                    UData.Text = selectedUser.dataNascita.ToString("dd/MM/yyyy");
                    UResidenza.Text = selectedUser.residenza;

                    // Controlla se l'utente è stato inserito manualmente  
                    string manualUserPath = @"../../script/UtenzeManuali/Iscrizioni_" + selectedUser.CodF + ".json";
                    bool isManualUser = File.Exists(manualUserPath);

                    UName.Enabled = isManualUser;
                    USurname.Enabled = isManualUser;
                    UEmail.Enabled = isManualUser;
                    UCodF.Enabled = isManualUser;
                    UTelefono.Enabled = isManualUser;
                    UGenere.Enabled = isManualUser;
                    UData.Enabled = isManualUser;
                    UResidenza.Enabled = isManualUser;
                }
            }
        }

        public void reload()
        {
            foreach (ListViewItem item in OfficeAbbBox.Items)
            {
                string nome = item.SubItems[0].Text;
                string cognome = item.SubItems[1].Text;

                User user = gameUsers.getActiveAbb().FirstOrDefault(u => u.user.nome == nome && u.user.cognome == cognome)?.user;

                if (user != null)
                {
                    item.SubItems[5].Text = "Attivo";
                }
                else
                {
                    item.SubItems[5].Text = "Non Attivo";
                }
            }

        }

        private void OfficeAbbBox_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                foreach (ListViewItem item in OfficeAbbBox.Items)
                {
                    if (item != e.Item && item.Checked)
                    {
                        item.Checked = false;
                    }
                }

                // Ottieni il nome e cognome dalla riga selezionata  
                string nome = e.Item.SubItems[0].Text;
                string cognome = e.Item.SubItems[1].Text;

                User s = gameUsers.findUser(nome, cognome);
                if (s == null)
                {
                    return;
                }

                UName.Text = s.nome;
                USurname.Text = s.cognome;
                UEmail.Text = s.email;
                UCodF.Text = s.CodF;
                UTelefono.Text = s.numeroTelefono.ToString();
                UGenere.Text = s.gender;
                UData.Text = s.dataNascita.ToString();
                UResidenza.Text = s.residenza;

                // Controlla se l'utente è stato inserito manualmente  
                string manualUserPath = @"../../script/UtenzeManuali/Iscrizioni_" + s.CodF + ".json";
                if (File.Exists(manualUserPath))
                {
                    // Rendi tutte le TextBox modificabili  
                    UName.Enabled = true;
                    USurname.Enabled = true;
                    UEmail.Enabled = true;
                    UCodF.Enabled = true;
                    UTelefono.Enabled = true;
                    UGenere.Enabled = true;
                    UData.Enabled = true;
                    UResidenza.Enabled = true;
                }
                else
                {
                    // Rendi tutte le TextBox non modificabili  
                    UName.Enabled = false;
                    USurname.Enabled = false;
                    UEmail.Enabled = false;
                    UCodF.Enabled = false;
                    UTelefono.Enabled = false;
                    UGenere.Enabled = false;
                    UData.Enabled = false;
                    UResidenza.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Controlla se le TextBox sono modificabili  
            if (UName.Enabled && USurname.Enabled && UEmail.Enabled && UCodF.Enabled &&
                UTelefono.Enabled && UGenere.Enabled && UData.Enabled && UResidenza.Enabled)
            {
                // Mostra un MessageBox per confermare il salvataggio  
                DialogResult result = MessageBox.Show("Vuoi salvare le modifiche apportate all'utente?", "Conferma Salvataggio", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Crea un nuovo oggetto User con i dati aggiornati  
                    User updatedUser = new User(
                        UName.Text,
                        USurname.Text,
                        UEmail.Text,
                        UCodF.Text,
                        UTelefono.Text,
                        UGenere.Text,
                        DateTime.Parse(UData.Text),
                        UResidenza.Text
                    );

                    // Salva i dati aggiornati nel file JSON  
                    string path = @"../../script/UtenzeManuali/Iscrizioni_" + updatedUser.CodF + ".json";
                    try
                    {
                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(updatedUser, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(path, json);
                        MessageBox.Show("Dati salvati con successo in: " + path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Errore durante il salvataggio dei dati: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Modifiche annullate.");
                }
            }
            else
            {
                MessageBox.Show("Le TextBox non sono modificabili.");
            }


        }

        private void TogglePanel1Button_Click(object sender, EventArgs e)
        {
            TogglePanel1();
        }
    }
}
