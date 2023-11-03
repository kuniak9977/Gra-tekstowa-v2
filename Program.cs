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
            Mechanics mechanics = new Mechanics();
            Console.Title = "MazeRunner Escape!";
            mechanics.RunMainMenu();
            

            //Font.UstawCzcionke(20,32);

            
            

        }
    }
}