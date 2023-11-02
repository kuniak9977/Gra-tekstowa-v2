using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public class Rooms
    {
        //StreamReader sr = new StreamReader("rooms.txt");
        //StringBuilder sb = new StringBuilder();
        public List<char[,]> rooms = new List<char[,]>();

        public Rooms()
        {
            string[] lines = File.ReadAllLines("rooms.txt");
            int numRows = lines.Length;
            int numCol = lines[0].Length;
            char[,] roomschars = new char[numRows, numCol];

            for (int i = 0; i < numRows; i++)
            {
                char[] chars = lines[i].ToCharArray();
                for (int j = 0; j < numCol; j++)
                {
                    roomschars[i, j] = chars[j];
                }
            }
            rooms.Add(roomschars);
            /*
            List<char> chars = new List<char>();
            while (!sr.EndOfStream)
            {
                char znak = (char)sr.Read();
                if (znak == '\n')
                    continue;
                if (znak == '+')
                {
                    char[] znakpokoje = chars.ToArray();
                    rooms.Add(znakpokoje);
                    chars.Clear();
                    continue;
                }
                chars.Add(znak);
            }*/
        }
    }
}
