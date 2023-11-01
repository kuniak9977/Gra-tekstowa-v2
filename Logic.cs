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

        public void ClearLastPosition(Rooms pokoj)
        {
            GetTypeOfObject(this);
            int lastX = temporary.roundedX;
            int lastY = temporary.roundedY;
            char[,] znaki = pokoj.rooms[0];
            char LastChar = znaki[temporary.roundedY, temporary.roundedX];
            Console.SetCursorPosition(lastX, lastY);
            Console.Write(LastChar);
            if (temporary is Entity.Player)
            {
                LastChar = znaki[temporary.roundedY - 1, temporary.roundedX];
                Console.SetCursorPosition(lastX, lastY - 1);
                Console.Write(LastChar);
            }

        }

        public bool ColisionWithWall(Rooms pokoj)
        {
            GetTypeOfObject (this);
            Rooms rooms = pokoj;
            char[,] znaki = rooms.rooms[0];
            char znak = znaki[temporary.roundedY, temporary.roundedX];
            return znak == '█';
        }


    }
}
