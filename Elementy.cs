using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public class Projectile
    {
        //public Position pos;
        double X, Y;
        int roundedX, roundedY;
        char shoot = '*';
        public double speed;
        public int damage;
        string direction;

        public Projectile(int x, int y, string direction)
        {
            this.X = x;
            this.Y = y;
            this.speed = 1;
            this.damage = 1;
            this.direction = direction;
        }

        public void DrawProjectile()
        {
            Console.SetCursorPosition(roundedX, roundedY);
            Console.Write(shoot);
        }
        public bool Kolizja(Rooms pokoj)
        {
            Rooms rooms = pokoj;
            int index = 0;
            for (int i = 0; i <= roundedY; i++)
            {
                for (int j = 0; j <= roundedX; j++)
                {
                    index++;

                }
            }
            char[] tablicaznakow = rooms.rooms[0];
            char wybranyznak = tablicaznakow[index];
            if (wybranyznak == '█')
            {
                
            }
        }
        public void Update(Rooms pokoj)
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
        public List<Projectile> projectiles;

        public Player()
        {
            this.pos = new Position(10, 10);
            this.exp = 0;
            this.lvl = 0;
            this.HealthPoints = 3;
            this.lookingdirection = "east";
            this.projectiles = new List<Projectile>();
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
