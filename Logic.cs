using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra_tekstowa_v2
{
    public abstract class Logic
    {
        private dynamic temporary = null;
        private void GetTypeOfObject(Object obj)
        {
            Type type = obj.GetType();
            if (type == typeof(Projectile))
            {
                temporary = (Projectile)obj;
            }
            else if (type == typeof(Entity.Player))
            {
                temporary = (Entity.Player)obj;
            } else
            {
                temporary = (Entity)obj;
            }
        }
        public void Update()
        {
            GetTypeOfObject(this);
            if (temporary != null)
            {
                switch (temporary.direction)
                {
                    case "east":
                        temporary.roundedX = (int)Math.Round(temporary.X += temporary.speed);
                        temporary.roundedY = (int)Math.Round(temporary.Y);
                        break;
                    case "west":
                        temporary.roundedX = (int)Math.Round(temporary.X -= temporary.speed);
                        temporary.roundedY = (int)Math.Round(temporary.Y);
                        break;
                    case "north":
                        temporary.roundedX = (int)Math.Round(temporary.X);
                        temporary.roundedY = (int)Math.Round(temporary.Y -= 0.5 * temporary.speed);
                        break;
                    case "south":
                        temporary.roundedX = (int)Math.Round(temporary.X);
                        temporary.roundedY = (int)Math.Round(temporary.Y += 0.5 * temporary.speed);
                        break;
                }
            }
        }

        public bool ColisionWithWall(char[,] chars)
        {
            GetTypeOfObject (this);
            char[,] znaki = chars;
            char znak = znaki[temporary.roundedY, temporary.roundedX];
            return znak == '█';
        }

        public bool EnterTheDoor(char[,] chars)
        {
            GetTypeOfObject(this);
            char[,] znaki = chars;
            char znak = znaki[temporary.roundedY,temporary.roundedX];
            return znak == '0';
        }


    }
}
