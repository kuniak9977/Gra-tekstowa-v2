using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using static Gra_tekstowa_v2.Entity;

namespace Gra_tekstowa_v2
{
    internal class Program
    {
        private static object playerLock = new object();
        private static object roomLock = new object();
        static void Main(string[] args)
        {

            Render render = new Render();
            Entity.Player player = new Entity.Player();
            List<Projectile> bullets = new List<Projectile>();
            List<Entity> entities = new List<Entity>();
            Random rnd = new Random();
            GenerateEntities();

            ConsoleKeyInfo cki;
            ConsoleKey keyPressed;
            Console.CursorVisible = false;

        


            Thread screeThread = new Thread(ScreenRefresh);
            Thread playerThread = new Thread(PlayerAction);
            screeThread.Start();
            playerThread.Start();

            void GenerateEntities()
            {
                
                int EntityCount = rnd.Next(2,10);
                for (int i = 0; i < EntityCount; i++)
                {
                    int X = rnd.Next(2,61);
                    int Y = rnd.Next(5,26);
                    Entity entity = new Entity("hard",X,Y);
                    entities.Add(entity);
                }
                foreach (Entity entity in entities)
                {
                    entity.DrawEntity();
                }
            }

            void UpdateEntities()
            {
                int RandomNumber = rnd.Next(101);

                for (int i = 0; i < entities.Count; i++)
                {
                    Entity entity = entities[i];
                    if (entity.CheckHealth())
                    {
                        entities.Remove(entity);
                        entity.ClearLastPosition(render.rooms);
                        i--;
                    }
                    //if ((player.roundedX == entity.roundedX) || player.roundedY == entity.roundedY)
                }
            }


            void ScreenRefresh()
            {
                int frame = 0;
                render.DrawRoom();
                player.DrawPlayer();
                GenerateEntities();
                while (true)
                {
                    //render.DrawRoom();
                    //player.ClearLastPosition(render.rooms);
                    ProjectileRefresh();
                    EntitiesRefesh();
                    Thread.Sleep(32);
                    //if (frame++ == 60)
                    //{
                    //    frame = 0;
                    //    render.DrawRoom();
                    //}
                }
            }

            void ProjectileRefresh()
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    Projectile p = bullets[i];
                    p.ClearLastPosition(render.rooms);
                    p.Update();
                    if (p.ColisionWithWall(render.rooms) || p.ColisionWithEntity(entities))
                    {
                        bullets.Remove(p);
                        i--;
                    }
                    else
                        p.DrawProjectile();
                }
            }

            void EntitiesRefesh()
            {
                UpdateEntities();
                foreach (Entity entity in entities)
                {
                    //entity.DrawEntity();
                }
            }

            void PlayerAction()
            {
                while (true)
                {
                    cki = Console.ReadKey(true);
                    keyPressed = cki.Key;
                    player.ClearLastPosition(render.rooms);
                    //player.DrawPlayer();

                    switch (keyPressed)
                    {
                        case ConsoleKey.A:
                            //player.X -= 1;
                            player.roundedX = (int)Math.Round(player.X -= player.speed);
                            player.roundedY = (int)Math.Round(player.Y);
                            player.lookingdirection = "west";
                            if (player.ColisionWithWall(render.rooms)) { player.X += 1; player.roundedX += 1; }
                            player.DrawPlayer();
                            break;
                        case ConsoleKey.W:
                            //player.Y -= 1;
                            player.roundedX = (int)Math.Round(player.X);
                            player.roundedY = (int)Math.Round(player.Y -= player.speed);
                            player.lookingdirection = "north";
                            if (player.ColisionWithWall(render.rooms)) { player.Y += 1; player.roundedY += 1; }
                            player.DrawPlayer();
                            break;
                        case ConsoleKey.S:
                            //player.Y += 1;
                            player.roundedX = (int)Math.Round(player.X);
                            player.roundedY = (int)Math.Round(player.Y += player.speed);
                            player.lookingdirection = "south";
                            if (player.ColisionWithWall(render.rooms)) { player.Y -= 1; player.roundedY -= 1; }
                            player.DrawPlayer();
                            break;
                        case ConsoleKey.D:
                            //player.X += 1;
                            player.roundedX = (int)Math.Round(player.X += player.speed);
                            player.roundedY = (int)Math.Round(player.Y);
                            player.lookingdirection = "east";
                            if (player.ColisionWithWall(render.rooms)) { player.X -= 1; player.roundedX -= 1; }
                            player.DrawPlayer();
                            break;
                        case ConsoleKey.Spacebar:
                            Projectile bullet = new Projectile(player.roundedX, player.roundedY, player.lookingdirection, "player");
                            bullets.Add(bullet);
                            player.DrawPlayer();
                            break;
                    }
                    Thread.Sleep(32);
                }
            }
        }
    }
}