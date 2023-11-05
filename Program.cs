using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Gra_tekstowa_v2.Mechanics;

namespace Gra_tekstowa_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Mechanics mechanics = new Mechanics();
            Console.Title = "MazeRunner Escape!";
            Font.UstawCzcionke(8, 16);
            MenuMain:
            mechanics.RunMainMenu();
            AplicationRunning:
            while (!mechanics.isPaused)
            {
                if (mechanics.exitGenerated)
                    goto End;
                if (mechanics.playerIsDead)
                    goto DeadScreen;
            }
            mechanics.PauseMenu();
            if (!mechanics.isPaused)
                goto AplicationRunning;

            End:
            mechanics.Stop();
            Console.Clear();
            mechanics.ExitMenu();
            goto MenuMain;

            DeadScreen:
            mechanics.Stop();
            Console.Clear();
            mechanics.DeadMenu();
            goto MenuMain;
            //mechanics.Stop();
            Console.Clear();
            Console.WriteLine("Po pętli");
        }
    }
}