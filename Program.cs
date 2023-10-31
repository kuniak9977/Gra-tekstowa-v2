using System.Security.Cryptography.X509Certificates;

namespace Gra_tekstowa_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Render render = new Render();
            Player player = new Player();
            List<Projectile> bullets = new List<Projectile>();

            ConsoleKeyInfo cki;
            ConsoleKey keyPressed;
            Console.CursorVisible = false;
            Thread screeThread = new Thread(ScreenRefresh);

            Thread playerThread = new Thread(PlayerAction);
            screeThread.Start();
            playerThread.Start();

            void ScreenRefresh()
            {
                render.DrawRoom();
                while (true)
                {
                    //Console.Clear();
                    player.DrawPlayer();
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        Projectile p = bullets[i];
                        p.ClearLastPosition(render.rooms);
                        p.Update();
                        if (p.ColisionWithWall(render.rooms))
                        {
                            bullets.Remove(p);
                            i--; // zmniejsz indeks, ponieważ usunięto element, a reszta przesunęła się o jeden w lewo
                        }
                        else
                            p.DrawProjectile();
                    }
                    Thread.Sleep(32);
                }
            }

            void PlayerAction()
            {
                while (true)
                {
                    cki = Console.ReadKey(true);
                    keyPressed = cki.Key;
                    player.ClearLastPosition(render.rooms);
                    switch (keyPressed)
                    {
                        case ConsoleKey.A:
                            //player.X -= 1;
                            player.roundedX = (int)Math.Round(player.X -= player.speed);
                            player.roundedY = (int)Math.Round(player.Y);
                            player.lookingdirection = "west";
                            break;
                        case ConsoleKey.W:
                            //player.Y -= 1;
                            player.roundedX = (int)Math.Round(player.X);
                            player.roundedY = (int)Math.Round(player.Y -= player.speed);
                            player.lookingdirection = "north";
                            break;
                        case ConsoleKey.S:
                            //player.Y += 1;
                            player.roundedX = (int)Math.Round(player.X);
                            player.roundedY = (int)Math.Round(player.Y += player.speed);
                            player.lookingdirection = "south";
                            break;
                        case ConsoleKey.D:
                            //player.X += 1;
                            player.roundedX = (int)Math.Round(player.X += player.speed);
                            player.roundedY = (int)Math.Round(player.Y);
                            player.lookingdirection = "east";
                            break;
                        case ConsoleKey.Spacebar:
                            Projectile bullet = new Projectile(player.roundedX, player.roundedY, player.lookingdirection);
                            bullets.Add(bullet);
                            break;
                    }
                    Thread.Sleep(32);
                }
            }
        }
    }
}