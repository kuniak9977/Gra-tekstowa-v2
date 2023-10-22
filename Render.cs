using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public class Render
    {
        public int width;
        public int height;
        public Rooms rooms;

        public Render()
        {
            width = 64;
            height = 27;
            Console.WindowHeight = height;
            Console.WindowWidth = width;
            Console.BufferHeight = height;
            Console.BufferWidth = width;
            this.rooms = new Rooms();
        }

        public void DrawRoom()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            char[] room = rooms.rooms[0];
            Console.Write(room);
        }
    }
}
