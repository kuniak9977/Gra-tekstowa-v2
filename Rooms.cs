using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public class Rooms
    {
        public List<char[,]> room = new List<char[,]>();

        public Rooms()
        {
            //string[] lines = File.ReadAllLines("C:\\Users\\kuniak9977\\Desktop\\projekt tekstowy kck ps\\Gra tekstowa v2\\bin\\Debug\\net6.0\\rooms.txt");
            string[] lines = File.ReadAllLines("rooms.txt");
            int numRows = lines.Length;
            int numCol = lines[0].Length;
            char[,] roomschars = new char[27, 64];
            int index = 0;
            for (int i = 0; i < numRows; i++)
            {
                char[] chars = lines[i].ToCharArray();
                for (int j = 0; j < numCol; j++)
                {
                    if (chars[j] == '?')
                        roomschars[index, j] = ' ';
                    else if (chars[j] == '=')
                    {
                        room.Add(roomschars);
                        
                        j = -1;
                        index = 0;
                        if (i + 1 == numRows)
                            break;
                        roomschars = new char[27, 64];
                        chars = lines[++i].ToCharArray();
                    }
                    else
                        roomschars[index, j] = chars[j];
                }
                index++;
            }
        }
    }
}
