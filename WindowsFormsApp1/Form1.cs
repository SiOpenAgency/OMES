using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Operazioni;

//using qualcosa che legga i dll

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        MainClass m = new MainClass();
        public Form1()
        {
            InitializeComponent();
        }

        private void drag(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void drop(object sender, DragEventArgs e)
        {
            
            var s = (string[])e.Data.GetData(DataFormats.FileDrop);
            FileInfo si = new FileInfo(s[0]);
            if (si.Extension == ".pdf")
            {
                if (File.Exists("temp.xls"))
                    File.Delete("temp.xls");
                m.PDFToExcel(s[0], "temp.xls");
                panel1.BackColor = Color.LightBlue;
            }
            else
            {
                MessageBox.Show("Inserire un file.pdf", "Errore");
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
       
            //Creo una istanza di Impostazioni e chiamo il metodo che carica i dati dal json
            List<Impostazioni> settings = DataManager.LoadSettings();
            comboBox1.DataSource = settings;
            comboBox1.DisplayMember = "NomeAzienda";
            

        }

  

        private void btnInvio_Click(object sender, EventArgs e)
        {

            btnInvio.Enabled = false;

            Impostazioni imp = (Impostazioni)comboBox1.SelectedItem;
            
            Visualizzatore vs = new Visualizzatore(imp);
            vs.ShowDialog();

            if (vs.valoreFlag == true)
            {
                btnInvio.Enabled = true;
                //Qui tolgo il colore alla box dove droppo il file
                panel1.BackColor = Color.Empty;

            }
            else
            {
                Thread.Sleep(10000);
                btnInvio.Enabled = true;
                //Qui tolgo il colore alla box dove droppo il file
                panel1.BackColor = Color.Empty;
            }

        }


        private void Esci_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApriEsploraRisorse(object sender, EventArgs e)
        {
            MainClass m = new MainClass();
            var pathFileSelezionato = string.Empty;
            var contenutoFileSelezionato = string.Empty;

            OpenFileDialog apriEsplora = new OpenFileDialog();
            apriEsplora.Filter = "pdf files (*.pdf)|*.pdf";
            //apriEsplora.FilterIndex = 1;   //Non dovrebbe servire perchè il valore predefinito è 1
            apriEsplora.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //Inizio salvataggio path file
            if (apriEsplora.ShowDialog() == DialogResult.OK)
            {
                //prendo il path
                pathFileSelezionato = apriEsplora.FileName;

                //leggo il contenuto del file dentro uno stream                 // Non mi serve leggere lo stream dei dati perchè 
                /*                                                              // il dll farà il resto e converte
                var fileStream = apriEsplora.OpenFile();                        // 
                                                                                //
                using(StreamReader reader = new StreamReader(fileStream))       //
                {                                                               //
                    contenutoFileSelezionato = reader.ReadToEnd();              //
                } */
                panel1.BackColor = Color.LightBlue;
            }
            //MessageBox.Show("Il path del file è: " + pathFileSelezionato);

            //conversione file attraverso API -> 
            //non c'è la chiamata alla funzione drop perchè la chiamata richiede altri paramtetri(?)
            var s = pathFileSelezionato;
            if (File.Exists("temp.xls")) //se il file esiste lo elimina -> così da evitare conflitti e da avere sempre il file nuovo
                File.Delete("temp.xls");


            m.PDFToExcel(s, "temp.xls");


        }
    }
}
