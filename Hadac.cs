/************************************************************************************************
 * Šibenice
 * Třída Hadac.cs - generuje hádaná písmena
 * 
 * Lukáš Haringer
 * 7.5.2015
 * verze 2.0 
 ***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibenice
{
    class Hadac
    {
        static Random rnd = new Random();
        IList<string> slovnik;
        IList<char> avaibleChars;

        /*Vrací list všech písmen, které lze zadat*/
        public List<char> getAvaibleCharsList()
        {
            List<char> list = new List<char>();
            list.Add('A');
            list.Add('B');
            list.Add('C');
            list.Add('Č');
            list.Add('D');
            list.Add('Ď');
            list.Add('E');
            list.Add('F');
            list.Add('G');
            list.Add('H');
            list.Add('I');
            list.Add('J');
            list.Add('K');
            list.Add('L');
            list.Add('M');
            list.Add('N');
            list.Add('Ň');
            list.Add('O');
            list.Add('P');
            list.Add('Q');
            list.Add('S');
            list.Add('Š');
            list.Add('R');
            list.Add('Ř');
            list.Add('T');
            list.Add('Ť');
            list.Add('U');
            list.Add('V');
            list.Add('W');
            list.Add('X');
            list.Add('Y');
            list.Add('Z');
            list.Add('Ž');
            return list;
        }


        /*Kontruktor třídy Hadac
          Nastaví list avaibleChars a slovnik
         */
        public Hadac() {
            avaibleChars=getAvaibleCharsList();
            slovnik=IO.nactiSlovnik();
        }

        /*Vyřadí ze slovníku všechna slova, která neodpovídají délkou masce*/
        public void vyradDleDelky(){
            List<string> temp = new List<string>(); 
            foreach (string slovo in slovnik)
            {
                if (slovo.Length == Game.mask.Length)
                {
                    temp.Add(slovo.ToUpper());
                }
            }
            slovnik = temp;
        }

        /*Vyřadí ze slovníku všechna slova, která se v jakékoli pozici známého znaku neshoduje s maskou*/
        public void vyradDleMasky()
        {
            List<string> temp = new List<string>();
            foreach (string slovo in slovnik)
            {
                for (int i = 0; i < Game.mask.Length;i++)
                {
                    if (Game.mask[i] == '?')
                    {
                        if (i == Game.mask.Length - 1) { temp.Add(slovo); }
                        else { continue; }
                    }
                    else if (Game.mask[i]==slovo[i])
                    {
                        if (i == Game.mask.Length - 1) { temp.Add(slovo); }
                        else { continue; }
                    }
                    else { break; }
                }
                
            }
            slovnik = temp;
        }

        /*Vrátí náhodné nepoužité písmeno*/
        public char getRandomChar()
        {
            char znak;

            do
            {
                znak = avaibleChars.ElementAt(rnd.Next(30));
            }

            while (Game.chosedChars.Contains(znak));
            return znak;
        }

        /*Vrátí písmeno s největší četností ve slovníku, pokud je slovník prázdný vrátí náhodné písmeno*/
        public char getChar()
        {
            int maxValue;
            int maxIndex;
            int[] vyskyty = new int[(int)char.MaxValue];

            if (!slovnik.Any())
            {
               return getRandomChar();
            }

            foreach (string slovo in slovnik)
            {
                foreach (char c in slovo)
                {
                        vyskyty[(int)c]++;
                } 
            }

            do
            {
                maxValue = vyskyty.Max();

                if (maxValue==0)
                {
                    return getRandomChar();
                }
                maxIndex = vyskyty.ToList().IndexOf(maxValue);
                vyskyty[maxIndex] = 0;
            }
            while (Game.chosedChars.Contains((char)maxIndex));
            return (char)maxIndex;
        }

        }


    }

