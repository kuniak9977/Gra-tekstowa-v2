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
        string source;
        public double X, Y;
        public int roundedX, roundedY;
        public double speed;
        public int damage;
        public string direction;


        public Projectile(int x, int y, string direction, string source)
        {
            this.source = source;
            this.speed = 1;
            this.damage = 1;
            this.direction = direction;
            this.X = x;
            this.Y = y;
        }

        public bool ColisonWithPlayer(Entity.Player player)
        {
            return (player.roundedX == this.roundedX) && (player.roundedY == this.roundedY) && (this.source == "entity");
        }

        public bool ColisionWithEntity(List<Entity> lista)
        {
            foreach (var entity in lista)
            {
                if ((this.roundedX == entity.roundedX) && (this.roundedY == entity.roundedY) && (this.source == "player"))
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
        public char symbol = 'E';
        public double X, Y;
        public int roundedX, roundedY;
        public double speed = 0.5;
        private string difficulty;
        public string lookingdirection;

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

        public bool ZeroHealth()
        {
            if (this.HealthPoints <= 0)
                return true;
            return false;
        }

        public class Player : Entity
        {
            public int exp;
            public int lvl;

            public Player() : base ()
            {
                this.speed = 1;
                this.X = 10;
                this.Y = 10;
                this.roundedX = 10;
                this.roundedY = 10;
                this.exp = 0;
                this.lvl = 0;
                this.HealthPoints = 3;
                this.lookingdirection = "east";
            }

            public bool ColisionWithEntity (List<Entity> lista)
            {
                foreach (Entity e in lista)
                {
                    if (e.roundedY == this.roundedY && e.roundedX == this.roundedX)
                    {
                        //this.HealthPoints--;
                        return true;
                    }
                }
                return false;
            }


        }
    }
}
