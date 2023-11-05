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
            mechanics.RunMainMenu();
            AplicationRunning:
            while (!mechanics.isPaused)
            {

            }
            mechanics.PauseMenu();
            if (!mechanics.isPaused)
                goto AplicationRunning;
            //mechanics.Stop();
            Console.Clear();
            Console.WriteLine("Po pętli");
            //mechanics.PauseMenu();
            //if (mechanics.isPaused)
            //    goto ScreenRefresh;

            //Font.UstawCzcionke(20,32);




        }
    }
}