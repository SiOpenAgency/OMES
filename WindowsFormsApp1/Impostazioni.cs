using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public class Impostazioni
    {

        public string NomeAzienda { get; set; }
        public string[] Parametri { get; set; }
        public int Distanza { get; set; }
        public int Limite { get; set; }

    }



     
    public class DataManager
    {
        public static List<Impostazioni> LoadSettings(String path = "C:\\Users\\Fabio\\source\\repos\\WindowsFormsApp1\\Impostazioni.json") //qui metto il path "default", se poi lo voglio cambiare passo un'altra stringa
        {
            try
            {
                return JsonSerializer.Deserialize<List<Impostazioni>>(File.ReadAllText(path));
            }
            catch (FileNotFoundException e)
            {
                FileInfo fi = new FileInfo(path);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                if (File.Exists(path))
                {
                    File.WriteAllText(path, JsonSerializer.Serialize<List<Impostazioni>>(new List<Impostazioni>()));
                }
                return new List<Impostazioni>();
            }

            
        } //fine LoadSettings
     }

   }

