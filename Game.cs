/************************************************************************************************
 * Šibenice
 * Třída Game.cs - windows form - obsahuje metody hry
 * 
 * Lukáš Haringer
 * 7.5.2015
 * verze 2.0 
 ***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sibenice
{   
    public partial class Game : Form
    {
        Panel currentPanel;
        /*Hádané slovo*/
        private string word;
        /*Odkaz na instanci hadace*/
        private static Hadac hadac;
        /*Flag zda se jedná o hru proti počítači nebo jen částečnou hru*/
        private static bool fullGame = false;
        /*Proměná pro uložení počtu udělaných člověkem při hře proti počítači*/
        private static int bodyPorovnani = -1;
        /*Proměná pro uložení počtu*/
        private int error;
        /*Proměná pro uložení masky slova*/
        public static string mask = "";
        /*Flag zda se podařilo načíst obrázek, pokud ne je konec hry*/
        bool ok = true;
        /*List použitých písmen*/
        public static IEnumerable<char> chosedChars = new List <char>();

        /*Konstruktor okna Game, vytvoří okno a nastaví jako viditelný panelHlavni*/
        public Game()
        {
            InitializeComponent();
            currentPanel = panelHlavni;
            this.Controls.Add(currentPanel);
        }
        
        /*Spustí hru hádání slova*/
        private void startHuman_Click(object sender, EventArgs e)
        {
            fullGame = false;
            bodyPorovnani = -1;
            gameStart();
        }

        /*Spustí hru zadání slova*/
        private void startPc_Click(object sender, EventArgs e)
        {
            fullGame = false;
            bodyPorovnani = -1;
            gameStartPc();
        }

        /*Spustí hru proti počítači*/
        private void fullGameButton_Click(object sender, EventArgs e)
        {
            fullGame = true;
            bodyPorovnani = -1;
            gameStart();
        }

        /*Přepne panel na panelPc a připraví proměnné pro hru zadání slova*/
        private void gameStartPc()
        {   
            this.Controls.Remove(currentPanel);
            currentPanel = panelPc;
            this.Controls.Add(currentPanel);

            okButton.Enabled = true;
            hadejButton.Enabled = false;
            lblWordPc.Text = "";
            labelPismena.Text = "";
            textBoxSlovo.Text = "";
            error = 0;
            ok = IO.loadPicture(error, pictureBox2);
            chosedChars = new List<char>();
        }

        /*Přepne panel na panelHrac a připraví proměnné pro hru hádání slova*/
        private void gameStart()
        {
            this.Controls.Remove(currentPanel);
            currentPanel = panelHrac;
            this.Controls.Add(currentPanel);
            showAll();
            word = IO.getWord();
            chosedChars = new List<char>();
            error = 0;
            lblWord.Text = getMask();
            ok = IO.loadPicture(error, pictureBox1);
        }

        /*Nastaví všechny prvky control na visible*/
        private void showAll() { 
            foreach (Control ctrn in panel3.Controls){
                ctrn.Visible=true;
            }
        }

        /*Vrátí masku v závislo na hádaném slově a již hádaných písmenech*/
        public string getMask()
        {
            string maska = "";
            foreach (char c in word)
            {
                if (c == ' ' || chosedChars.Contains(c))
                    maska = maska + c;

                else
                    maska = maska + "?";
            }
            mask = maska;
            return maska;
        }

        /*Vrátí list písmen, podle hádaného znaku (písmena zastupují i písmena s diakritikou)
          param c hádaný znak
         */
        private IList<char> getCharList(char c) { 
        IList<char> list = new List<char>();

        list.Add(c);

            switch(c){
                case('I'):
                    list.Add('Í');
                    break;
                case('Y'):
                    list.Add('Ý');
                    break;
                case('O') :
                    list.Add('Ó');
                    break;
                case('E'): 
                    list.Add('Ě');
                    list.Add('É');
                    break;
                case('A'):
                    list.Add('Á');
                    break;
                case ('U'): 
                    list.Add('Ú');
                    list.Add('Ů');
                    break;
                default:break;
            }

            return list;  
        }

        /*Metoda pro určení zda je daný znak ve slově*/
        private bool Hit(IList<char> list)
        {
            foreach(char c in word){
                if(list.Contains(c)) return true;
            }
        return false;
        }

        /*Metoda pro zadání špatného písmene, načte nový obrázek a zvedne počet chyb*/
        private void Miss()
        {
            error++;
            ok = IO.loadPicture(error,pictureBox1);
            if (!ok) { gameOver(); }
        }

        /*Metoda pro zadání špatného písmene, načte nový obrázek a zvedne počet chyb*/
        private void MissPc()
        {
            error++;
            ok = IO.loadPicture(error, pictureBox2);
            if (!ok) { gameOver(); }
        }

        /*Metoda pro ověření zda jsou uhodnuty všechny znaky*/
        private Boolean win()
        {
            foreach (char c in word)
            {
                if (c != ' ' && !chosedChars.Contains(c))
                    return false;
            }
            return true;

        }

        /*Metoda pro vypsání výsledků po prohrané hře*/
        private void gameOver()
        {
            if (fullGame)
            {
                bodyPorovnani = error;
                fullGame = false;
                MessageBox.Show("Neuhodnuto!\n\n Slovo bylo: " + word); 
                gameStartPc();
            }
            else
            {
                if (bodyPorovnani == -1) {
                    MessageBox.Show("Neuhodnuto!\n\n Slovo bylo: " + word);
                }
                else{
                    if (bodyPorovnani >= error)
                    {
                        MessageBox.Show("Hráč udělal " + bodyPorovnani + " chyb \nPočítač udělal " + error + " chyb \n\nPočítač vyhrál");
                    }
                    else { MessageBox.Show("Hráč udělal " + bodyPorovnani + " chyb \nPočítač udělal " + error + "chyb \n\nHráč vyhrál"); }
                }
                this.Controls.Remove(currentPanel);
                currentPanel = panelHlavni;
                this.Controls.Add(currentPanel);
            }
        }

        /*Metoda pro vypsání výsledků po vyhranné hře*/
        private void gameWin()
        {
            if (fullGame)
            {
                bodyPorovnani = error;
                fullGame = false;
                MessageBox.Show("Uhodnuto! \n\n Počet chyb: " + error);
                gameStartPc();
            }
            else
            {
                if (bodyPorovnani == -1)
                {
                    MessageBox.Show("Uhodnuto! \n\n Počet chyb: " + error);
                }
                else
                {
                    if (bodyPorovnani >= error)
                    {
                        MessageBox.Show("Hráč udělal " + bodyPorovnani + " chyb \nPočítač udělal " + error + " chyb \n\nPočítač vyhrál");
                    }
                    else { MessageBox.Show("Hráč udělal " + bodyPorovnani + " chyb \nPočítač udělal " + error + "chyb \n\nHráč vyhrál"); }
                }
                this.Controls.Remove(currentPanel);
                currentPanel = panelHlavni;
                this.Controls.Add(currentPanel);
            }
        }

        /*Metoda volající se po stiknutí tlačítka písmene. Ověří zda písmeno je ve slově a poté buď změní masku nebo neobrázek*/
        private void pismenoClick(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            char c = btn.Text[0];
            IList<char> list = getCharList(c);
            bool hit = Hit(list);
            chosedChars = chosedChars.Concat(list);
            btn.Visible = false;
            if (hit) { 
                lblWord.Text = getMask();
                if (win()) { gameWin(); }
                }
            else Miss();
        }

        /*Metoda volající se po stisknutí tlačítka potvrzujícího zadání slova. 
         * Načte slovo a vygeneruje z nej masku. Vytvori instanci hadace
         */
        private void zacniHadat(object sender, EventArgs e)
        {
            word = textBoxSlovo.Text.ToUpper();

            Regex validace = new Regex("^[a-zA-Zá-ž+Á-Ž]*$");

            if (validace.IsMatch(word) && word!="")
            {
                okButton.Enabled = false;
                mask = getMask();
                lblWordPc.Text = mask;

                hadac = new Hadac(); 

                hadac.vyradDleDelky();
                hadejButton.Enabled = true;
           }
            else { MessageBox.Show("Slovo obsahuje zakázané znaky"); } 
            
        }

        /*Metoda volající se po stisknutí tlačítka další písmeno. Hadac vygeneruje písmeno pro hru, 
         * ověří se zda je písmeno ve slově a poté buď změní masku nebo neobrázek 
         */
        private void dalsiPismeno(object sender, EventArgs e)
        {
            char c = hadac.getChar();
            labelPismena.Text = labelPismena.Text + " " + c;

            IList<char> list = getCharList(c);
            chosedChars = chosedChars.Concat(list);
            bool hit = Hit(list);
            if (hit)
            {
                mask = getMask();
                lblWordPc.Text = mask;
                hadac.vyradDleMasky();
                if (win()) { gameWin(); }
            }
            else { MissPc(); }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {


        }  
    }
}
