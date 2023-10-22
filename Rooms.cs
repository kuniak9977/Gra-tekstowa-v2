using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public class Rooms
    {
        StreamReader sr = new StreamReader("rooms.txt");
        StringBuilder sb = new StringBuilder();
        public List<char[]> rooms = new List<char[]>();

        public Rooms()
        {
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
            }
        }
    }
}
