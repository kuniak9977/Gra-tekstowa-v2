using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public class Render
    {
        public int width;
        public int height;
        public Rooms rooms;
        public Map map;
        int mapSize;

        public StringBuilder stringbuilder = new StringBuilder();

        public Render(string difficulty)
        {
            width = 64;
            height = 27;
            Console.WindowHeight = height;
            Console.WindowWidth = width;
            Console.BufferHeight = height;
            Console.BufferWidth = width;
            this.rooms = new Rooms();
            switch (difficulty)
            {
                case "easy":
                    mapSize = 6;
                    break;
                case "medium":
                    mapSize = 8;
                    break;
                case "hard":
                    mapSize = 10;
                    break;
            }
            this.map = new Map(rooms,mapSize);
            map.GenrateMap();
        }


        public void LoadRoom(int playerlocation)
        {
            //Console.Clear();
            //Console.SetCursorPosition(0, 0);
            char[,] roomschar = map.RoomList[playerlocation].roomchar;
            int numRows = roomschar.GetLength(0);
            int numCol = roomschar.GetLength(1);
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCol; j++)
                {
                    if (roomschar[i, j] == '+')
                        roomschar[i, j] = ' ';
                    stringbuilder.Append(roomschar[i,j]);
                }
            }
        }

        public StringBuilder GetStringBuilder()
        {
            return stringbuilder;
        }
    }

    public class Map
    {
        public List<Node> RoomList = new List<Node>();
        Rooms AvaibleRooms;
        int size;
        public Map(Rooms rooms,int size)
        {
            this.AvaibleRooms = rooms;
            this.size = size;
        }

        public void GenrateMap()
        {
            Random rnd = new Random();
            int i = 0;
            while (RoomList.Count < size)
            {
                if (i >= size)
                    i = 0;
                int RoomType = rnd.Next(0, AvaibleRooms.room.Count);

                if (RoomList.Count == 0)
                {
                    Node room = new Node(AvaibleRooms.room[RoomType]);
                    RoomList.Add(room);
                }

                if (RoomList[i].Neighbours.Count < 4)
                {
                    int currentConection = RoomList[i].Neighbours.Count;
                    int roomsToAdd = rnd.Next(2, 4);
                    int toAdd = roomsToAdd - currentConection;
                    int allRooms = RoomList.Count + toAdd;

                    if (allRooms <= size)
                    {
                        for (int j = 1; j <= toAdd; j++)
                        {
                            RoomType = rnd.Next(0, AvaibleRooms.room.Count);
                            Node room = new Node(AvaibleRooms.room[RoomType]);
                            RoomList.Add(room);
                            int randDirection = rnd.Next(0, 3);
                            while (RoomList[i].direction[randDirection] != ' ')
                                randDirection = rnd.Next(0, 3);
                            int AddedRoomIndex = RoomList.Count - 1;
                            switch (randDirection)
                            {
                                case 0:
                                    RoomList[i].direction[0] = (char)AddedRoomIndex;
                                    RoomList[i].Neighbours.Add(AddedRoomIndex);
                                    RoomList[AddedRoomIndex].direction[1] = (char)i;
                                    RoomList[AddedRoomIndex].Neighbours.Add(i);
                                    break;
                                case 1:
                                    RoomList[i].direction[1] = (char)AddedRoomIndex;
                                    RoomList[i].Neighbours.Add(AddedRoomIndex);
                                    RoomList[AddedRoomIndex].direction[0] = (char)i;
                                    RoomList[AddedRoomIndex].Neighbours.Add(i);
                                    break;
                                case 2:
                                    RoomList[i].direction[2] = (char)AddedRoomIndex;
                                    RoomList[i].Neighbours.Add(AddedRoomIndex);
                                    RoomList[AddedRoomIndex].direction[3] = (char)i;
                                    RoomList[AddedRoomIndex].Neighbours.Add(i);
                                    break;
                                case 3:
                                    RoomList[i].direction[3] = (char)AddedRoomIndex;
                                    RoomList[i].Neighbours.Add(AddedRoomIndex);
                                    RoomList[AddedRoomIndex].direction[2] = (char)i;
                                    RoomList[AddedRoomIndex].Neighbours.Add(i);
                                    break;
                            }
                        }
                    }
                }
                i++;
            }
        }

    }
    public class Node
    {
        public List<int> Neighbours;
        //                       north,south,east,west
        public char[] direction = { ' ', ' ', ' ', ' ' };
        public char[,] roomchar;
        public bool RoomIsClear;

        public Node(char[,] roomchar)
        {
            this.roomchar = roomchar;
            this.Neighbours = new List<int>();
            this.RoomIsClear = false;
        }
    }
}
