using System.Security.Cryptography.X509Certificates;

namespace Gra_tekstowa_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Render render = new Render();
            Player player = new Player();

            ConsoleKeyInfo cki;
            ConsoleKey keyPressed;
            Console.CursorVisible = false;
            Thread screeThread = new Thread(ScreenRefresh);
            Thread bulletsThread = new Thread(BulletsUpdate);
            Thread playerThread = new Thread(PlayerAction);
            screeThread.Start();
            playerThread.Start();
            bulletsThread.Start();



            void ScreenRefresh()
            {
                while (true)
                {
                    Console.Clear();
                    render.DrawRoom();
                    player.DrawPlayer();
                    foreach (var thing in player.projectiles)
                    {
                        thing.DrawProjectile();
                    }
                    Thread.Sleep(32);
                }
            }
            void BulletsUpdate()
            {
                while(true)
                {
                    foreach (var thing in player.projectiles)
                    {
                        thing.Update(render.rooms);
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
                    switch (keyPressed)
                    {
                        case ConsoleKey.A:
                            player.pos.x -= 1;
                            player.lookingdirection = "west";
                            break;
                        case ConsoleKey.W:
                            player.pos.y -= 1;
                            player.lookingdirection = "north";
                            break;
                        case ConsoleKey.S:
                            player.pos.y += 1;
                            player.lookingdirection = "south";
                            break;
                        case ConsoleKey.D:
                            player.pos.x += 1;
                            player.lookingdirection = "east";
                            break;
                        case ConsoleKey.Spacebar:
                            Projectile bullet = new Projectile(player.pos.x, player.pos.y, player.lookingdirection);
                            player.projectiles.Add(bullet);
                            break;
                    }
                    Thread.Sleep(32);
                }
            }
        }
    }
}