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
            //Thread bulletsThread = new Thread(BulletsUpdate);
            Thread playerThread = new Thread(PlayerAction);
            screeThread.Start();
            playerThread.Start();
            //bulletsThread.Start();



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
                        p.Update();
                        if (p.ColisionWithWall(render.rooms))
                        {
                            bullets.Remove(p);
                            i--; // zmniejsz indeks, ponieważ usunięto element, a reszta przesunęła się o jeden w lewo
                        }
                        else
                            p.DrawProjectile();
                    }/*
                    foreach (Projectile p in bullets)
                    {
                        p.Update();
                        if(p.ColisionWithWall(render.rooms))
                            bullets.Remove(p);
                        if (bullets.Contains(p))
                            p.DrawProjectile();
                    }*/
                    Thread.Sleep(32);
                }
            }
            void BulletsUpdate()
            {
                while(true)
                {
                    foreach (var thing in bullets)
                    {
                        thing.Update();
                        //if(thing.Kolizja(render.rooms))
                        //    player.projectiles.Remove(thing);
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
                            bullets.Add(bullet);
                            break;
                    }
                    Thread.Sleep(32);
                }
            }
        }
    }
}