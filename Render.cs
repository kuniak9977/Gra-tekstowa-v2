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

        public StringBuilder stringbuilder = new StringBuilder();

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

        public void LoadRoom()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            char[,] roomschar = rooms.rooms[0];
            int numRows = roomschar.GetLength(0);
            int numCol = roomschar.GetLength(1);
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCol; j++)
                {
                    stringbuilder.Append(roomschar[i,j]);
                }
            }
        }

        public StringBuilder GetStringBuilder()
        {
            return stringbuilder;
        }
    }
}
