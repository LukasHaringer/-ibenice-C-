/************************************************************************************************
 * Šibenice
 * Třída Program.cs - spuštěcí třída aplikace
 * 
 * Lukáš Haringer
 * 7.5.2015
 * verze 2.0 
 ***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sibenice
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Game());
        }
    }
}
