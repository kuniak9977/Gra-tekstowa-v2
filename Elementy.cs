using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public class Projectile : Logic
    {
        //public Position pos;
        string source;
        public double X, Y;
        public int roundedX, roundedY;
        char shoot = '*';
        public double speed;
        public int damage;
        public string direction;


        public Projectile(int x, int y, string direction, string source)
        {
            this.source = source;
            this.speed = 1;
            this.damage = 1;
            this.direction = direction;
            switch (direction)
            {
                case "south":
                    this.X = x;
                    this.Y = y;
                    break;
                case "north":
                    this.X = x;
                    this.Y = y;
                    break;
                case "west":
                    this.X = x;
                    this.Y = y;
                    break;
                case "east":
                    this.X = x;
                    this.Y = y;
                    break;
            }
        }

        public void DrawProjectile()
        {
            Console.SetCursorPosition(roundedX, roundedY);
            Console.Write(shoot);
        }

        public bool ColisionWithEntity(List<Entity> lista)
        {
            foreach (var entity in lista)
            {
                if ((this.roundedX == entity.roundedX) && (this.roundedY == entity.roundedY))
                {
                    entity.HealthPoints -= 1;
                    return true;
                }
            }
            return false;
        }

    }
    public class Entity : Logic
    {
        public double HealthPoints;
        char symbol = 'E';
        public double X, Y;
        public int roundedX, roundedY;
        public double speed = 1;
        private string difficulty;
        public Entity() { }

        public Entity(string difficulty, int X, int Y)
        {
            this.difficulty = difficulty;
            this.X = X; this.Y = Y;
            this.roundedX = X; this.roundedY = Y;
            switch (difficulty)
            {
                case "easy":
                    this.HealthPoints = 2;
                    break;
                case "medium":
                    this.HealthPoints = 4;
                    break;
                case "hard":
                    this.HealthPoints = 8;
                    break;
            }
        }
        public void DrawEntity()
        {
            Console.SetCursorPosition(roundedX, roundedY);
            switch (difficulty)
            {
                case "easy":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "medium":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "hard":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.Write(symbol);
            Console.ResetColor();
        }

        public bool ColisionWithPorojectile(Projectile projectile)
        {
            return (projectile.roundedX == this.roundedX) && (projectile.roundedY == this.roundedY);
        }

        public bool CheckHealth()
        {
            if (this.HealthPoints <= 0)
                return true;
            return false;
        }


        public class Player : Entity
        {
            public int exp;
            public int lvl;
            char head = 'Q';
            char body = 'X';
            public string lookingdirection;
            //public double X, Y;
            //public int roundedX, roundedY;
            //public double speed = 1;

            public Player() : base ()
            {
                //this.pos = new Position(10, 10);
                this.X = 10;
                this.Y = 10;
                this.roundedX = 10;
                this.roundedY = 10;
                this.exp = 0;
                this.lvl = 0;
                this.HealthPoints = 3;
                this.lookingdirection = "east";
            }

            public void DrawPlayer()
            {
                Console.SetCursorPosition(roundedX, roundedY);
                Console.Write(body);
                Console.SetCursorPosition(roundedX, roundedY - 1);
                Console.Write(head);
            }
        }
    }
}
