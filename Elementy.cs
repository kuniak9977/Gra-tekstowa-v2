using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public class Projectile// : IEquatable<Projectile>
    {
        //public Position pos;
        int id;
        double X, Y;
        int roundedX, roundedY;
        char shoot = '*';
        public double speed;
        public int damage;
        string direction;
        private HashSet<int> usedIds = new HashSet<int>();
        private Queue<int> recycledIds = new Queue<int>();

        public int GetUniqueId()
        {
            if (recycledIds.Count > 0)
            {
                int recycledId = recycledIds.Dequeue();
                usedIds.Add(recycledId);
                return recycledId;
            }
            else
            {
                int newId = usedIds.Count + 1;
                usedIds.Add(newId);
                return newId;
            }
        }
        public void RecycleId(int id)
        {
            if (usedIds.Contains(id))
            {
                usedIds.Remove(id);
                recycledIds.Enqueue(id);
            }
            else
            {
                throw new InvalidOperationException("Trying to recycle an ID that is not in use.");
            }
        }

        public Projectile(int x, int y, string direction)
        {
            this.X = x;
            this.Y = y;
            this.speed = 1;
            this.damage = 1;
            this.direction = direction;
            this.id = GetUniqueId();
        }

        public void DrawProjectile()
        {
            Console.SetCursorPosition(roundedX, roundedY);
            Console.Write(shoot);
        }
        
        public bool ColisionWithWall(Rooms pokoj)
        {
            Rooms rooms = pokoj;
            char[,] znaki = rooms.rooms[0];
            char znak = znaki[roundedY, roundedX];
            
            if (znak == '█')
                return true;
            return false;
        }
        public void Update()
        {
            
            switch (direction)
            {
                case "east":
                    roundedX = (int)Math.Round(X += speed);
                    roundedY = (int)Math.Round(Y);
                    break;
                case "west":
                    roundedX = (int)Math.Round(X -= speed);
                    roundedY = (int)Math.Round(Y);
                    break;
                case "north":
                    roundedX = (int)Math.Round(X);
                    roundedY = (int)Math.Round(Y -= speed);
                    break;
                case "south":
                    roundedX = (int)Math.Round(X);
                    roundedY = (int)Math.Round(Y += speed);
                    break;
            }
            
        }
    }
    public class Entity
    {
        public double HealthPoints;
        public int armor;
    }
    public class Player : Entity
    {
        public int exp;
        public int lvl;
        public Position pos;
        char head = 'Q';
        char body = 'X';
        public string lookingdirection;

        public Player()
        {
            this.pos = new Position(10, 10);
            this.exp = 0;
            this.lvl = 0;
            this.HealthPoints = 3;
            this.lookingdirection = "east";
        }

        public void DrawPlayer()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write(body);
            Console.SetCursorPosition(pos.x, pos.y - 1);
            Console.Write(head);
        }
    }
    public class Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
