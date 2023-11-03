using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public class Menu
    {
        private int WybranaOpcja;
        private string[] Opcje;
        private string Prompt;

        public Menu(string[] opcje, string prompt)
        {
            this.Opcje = opcje;
            this.Prompt = prompt;
            this.WybranaOpcja = 0;
        }

        private void ShowOptions()
        {
            int WidthSize = Console.WindowWidth;
            int HeightSize = Console.WindowHeight;
            int OptionQuantity = Opcje.Length;
            int MiddlePointX = WidthSize / 2;
            int MiddlePointY = (HeightSize / 3) - (OptionQuantity / 2);

            Console.SetCursorPosition(MiddlePointX - (Prompt.Length / 2),MiddlePointY);
            Console.Write(Prompt + "\n");
            for (int i = 0; i < Opcje.Length; i++)
            {
                string ObecnaOpcja = Opcje[i];
                string prefix, sufix;
                if (i == WybranaOpcja)
                {
                    prefix = ">>";
                    sufix = "<<";
                }
                else
                {
                    prefix = sufix = " ";
                }
                string ToWrite = $"{prefix} {ObecnaOpcja} {sufix}";
                Console.SetCursorPosition(MiddlePointX - (ToWrite.Length / 2), MiddlePointY + 1 + i);
                Console.WriteLine(ToWrite);
            }
        }

        public int Run()
        {
            ConsoleKey CK;
            do
            {
                Console.Clear();
                ShowOptions();

                ConsoleKeyInfo CKI = Console.ReadKey(true);
                CK = CKI.Key;

                if (CK == ConsoleKey.W)
                {
                    WybranaOpcja--;
                    if (WybranaOpcja < 0)
                        WybranaOpcja = Opcje.Length - 1;
                }
                if (CK == ConsoleKey.S)
                {
                    WybranaOpcja++;
                    if (WybranaOpcja == Opcje.Length)
                        WybranaOpcja = 0;
                }
            } while (CK != ConsoleKey.Enter);
            return WybranaOpcja;
        }

        
    }
}
