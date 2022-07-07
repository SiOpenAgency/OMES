using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;


namespace WindowsFormsApp1.Operazioni
{
    public class Excel
    {
        public string path;
        int Sheet = 1;
        public string primoValore = "";
        public string secondoValore = "";
        public _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;
        private int distanzaDaTestata;
        public List<Dato> dati = new List<Dato>();
        public Dictionary<string, int> dizionario = new Dictionary<string, int>();   //in questo dizionario il primo parametro sarà il valore da cercare mentre il secondo sarà l'indice della colonna al quale saà trovato

        public string[] lista;
        int limite;


        //--------PROVA NUOVO COSTRUTTORE-----------------
        public Excel(Impostazioni imp)
        {
            FileInfo fi = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            string primaPartePaath = fi.Directory.FullName;
            this.path = primaPartePaath + "\\temp.xls";


            this.distanzaDaTestata = imp.Distanza;
            wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[Sheet];


            lista = imp.Parametri;
            limite = imp.Limite;

        }


        //--------- INIZIO OPERAZIONI ---------------

        //la funzione permette di leggere le singole celle e restituisce la stringa contentente il contenuto delle celle stesse
        public string LeggiCella(int riga, int colonna)
        {
            try
            {
                return ws.Cells[riga, colonna].Value.ToString();
            }
            catch
            {
                return "";
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------

        public void CercaIndici()
        {
            foreach (string stringa in lista)
            {
                dizionario.Add(stringa, 0); //carico nel dictionary tutti i valori presenti all'interno delle proprietà del json
            }

            //faccio i for per scorrere le righe e le colonne
            //dentro ai for ci metto un foreach che mi scorre ogni valore all'interno dell'array contentente le stringhe dei parametri
            //e qui ci metto un if che confronta il valore dell'array con quello letto da "LeggiCella" al quale passo gli indici di riga e colonna

            Range used = ws.UsedRange;
            int colonna = 0;

            for (int i = 1; i < used.Rows.Count; i++)  //--> righe
            {
                AzzeraRigaDictionary();
                for (int j = 1; j < used.Columns.Count; j++)  //--> colonne
                {
                    foreach (string valore in lista)
                    {
                        if (LeggiCella(i, j) == valore)
                        {
                            //se trovo il valore da cercare mi salvo la colonna
                            colonna = j;
                            CaricaDictionary(colonna, valore);
                        }
                        if(!dizionario.Any(x=>x.Value == 0))     //se per tutte le chiavi ho un valore ->
                        {
                            CaricaDati(i + distanzaDaTestata);

                            //Forse qui dovrebbe andare il coso per azzerare il dictionary -> ho trovato 
                            //l'indice della colonna e l'ho salvato nel dictionary
                            //la riga è quella corrente + il valore di distanza dalla testata
                            //sono apposto(?)
                        }
                        //Se tutti i valori sono maggiori di 0 allora carica i dati   ---------> NON SO SE LAVORA PER TUTTI O SOLO PER UNO

                    }
                    /*
                                        if (!dizionario.Any(x=>x.Value==0))
                                        {
                                            CaricaDati(i);
                                        } */
                }

            }

        }

        public void CaricaDictionary(int colonna,string key)
        {
                dizionario[key] = colonna;
        }

        public void AzzeraRigaDictionary()
        {
            foreach (string stringa in lista)
            {
                dizionario[stringa] = 0;
            }
        }


        public void CaricaDati(int riga)
        {
            Range used = ws.UsedRange;

            string chiave = lista[0];    //-----> Il primo parametro della lista sarà sicuramente QUANTITA'
            int rigaChiave = riga;  //ho fatto meno distanza da testata perch+ quando chiamo questo metodo
            int colonnaChiave = 0;                      //gli passo la riga corrente + la distanza da testata, però a me serve la
            decimal quantita = 0;                       //riga alla quale trovo il valore di ricerca
            int colonnaRicerca = 0;

            int contatore = 0;
            int limit = limite;


            //IMPOSTO COLONNA_CHIAVE
            colonnaChiave = dizionario[chiave];
            //FINE IMPOSTO COLONNA_CHIAVE

            for (int i = riga; i < used.Rows.Count; i++)
            {

                var s = new Dato();
                if (decimal.TryParse(LeggiCella(i, colonnaChiave), out quantita)){  //qui controllo se c'è o meno una cella con quantità
                    contatore = 0;
                    
                    foreach(string key in lista)
                    {
                        colonnaRicerca = dizionario[key];
                        string ciao = LeggiCella(i, colonnaRicerca);
                        s.Parametri.Add(ciao);
                    }
                    dati.Add(s);

                }
                else
                {
                    contatore++;
                }
                if (contatore > limite) {
                    break;
                }
            }

            AzzeraRigaDictionary();


        }




        


    }//Classe Excel


} //Namespace


    






