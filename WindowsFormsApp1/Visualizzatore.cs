using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Operazioni;

namespace WindowsFormsApp1
{
    public partial class Visualizzatore : Form
    {
        public bool valoreFlag = false;
        public Visualizzatore(Impostazioni imp)
        {
            InitializeComponent();

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 4;
            progressBar1.Step = 1;


            Task t = Task.Factory.StartNew(() =>
            {
                Excel exc = new Excel(imp);  //passo i dati -> path ecc ecc



                exc.CercaIndici();
                exc.excel.Quit();

                Tabella.Invoke(new Action(() =>
                {

                    Tabella.Columns.Clear();
                    progressBar1.PerformStep();
                    Tabella.Rows.Clear();
                    progressBar1.PerformStep();

                    foreach (var s in imp.Parametri)
                    {
                        Tabella.Columns.Add(s, s);
                    }
                    progressBar1.PerformStep();
                    foreach (var s in exc.dati)
                    {
                        Tabella.Rows.Add(s.Parametri.ToArray());
                    }
                    progressBar1.PerformStep();
                    valoreFlag = true;

                }));

                
                MessageBox.Show("Operazione completata con successo", "Esito Operazione");
                progressBar1.Invoke(new Action(() => progressBar1.Value = 0));




            });



        }

    

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Tabella.Rows.Clear();
            Tabella.Columns.Clear();
        }
    }
}
