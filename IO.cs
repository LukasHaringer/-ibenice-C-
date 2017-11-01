/************************************************************************************************
 * Šibenice
 * Třída IO.cs - obsahuje metody pro vstup a výstup
 * 
 * Lukáš Haringer
 * 7.5.2015
 * verze 2.0 
 ***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sibenice
{
    class IO
    {
        public static Random random = new Random();

        /*Načte ze slovníku náhodné slovo*/
        public static string getWord()
        {
            string fn = getAppDir() + "\\slovnik.txt";
            if (File.Exists(fn))
            {
                string[] pokus = File.ReadAllLines(fn, Encoding.Default);
                int i = random.Next(0, pokus.Count() - 1);
                return pokus[i].ToUpper();
            }
            MessageBox.Show("Soubor nenalezen");
            return "Chyba";
        }

        /*Načte slova ze slovníku do List*/
        public static IList<string> nactiSlovnik()
        {
            IList<string> slovnik;
            string fn = getAppDir() + "\\slovnik.txt";
            if (File.Exists(fn))
            {
                slovnik = File.ReadAllLines(fn, Encoding.Default);
                return slovnik;
            }
            else
            {
                MessageBox.Show("Soubor se slovnikem nenalezen");
                return slovnik = new List<string>();
            }
        }

        /*Doplní k názvu souboru cestu k souboru*/
        public static string getAppDir()
        {
            FileInfo fi = new FileInfo(Application.ExecutablePath);
            return fi.DirectoryName;
        }

        /*Načte nový obrázek do zadaného pictureBox
         
         * param i cislo obrazku k nactenu
         * param pictureBox pictureBox do kterého se má obrázek načíst
         */
        public static bool loadPicture(int i,PictureBox pictureBox)
        {
            string dir = getAppDir() + "\\pics\\";
            string fn = dir + i.ToString().PadLeft(2, '0') + ".png";
            if (File.Exists(fn))
            {
                pictureBox.Image = new Bitmap(fn);
                return true;
            }
            return false;
        }   
    }
}
