using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Gra_tekstowa_v2.Entity;

namespace Gra_tekstowa_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Render render = new Render();
            Entity.Player player = new Player();
            List<Projectile> bullets = new List<Projectile>();
            List<Entity> entities = new List<Entity>();
            Random rnd = new Random();

            StringBuilder sb = new StringBuilder();
            render.DrawRoom();
            sb = render.GetStringBuilder();
            
            char[] chars = sb.ToString().ToCharArray();

            GenerateEntities();

            string difficulty = "easy";

            ConsoleKeyInfo cki;
            ConsoleKey keyPressed;
            Console.CursorVisible = false;


            Thread screeThread = new Thread(ScreenRefresh);
            Thread playerThread = new Thread(PlayerAction);
            screeThread.Start();
            playerThread.Start();


            void ScreenRefresh()
            {
                while (true)
                {
                    Console.SetCursorPosition(0, 0);
                    ShowLvlAndHP();
                    ClearPlayerPosiotion();
                    SetPlayerPosiotion();

                    //string String = sb.ToString();
                    //for (int i = 0; i < String.Length; i++)
                    //{
                    //    if (String[i] == 'E')
                    //        switch (difficulty)
                    //        {
                    //            case "easy":
                    //                Console.ForegroundColor = ConsoleColor.Green;
                    //                break;
                    //            case "medium":
                    //                Console.ForegroundColor = ConsoleColor.Yellow;
                    //                break;
                    //            case "hard":
                    //                Console.ForegroundColor = ConsoleColor.Red;
                    //                break;
                    //        }
                    //    Console.Write(String[i]);
                    //    Console.ResetColor();
                    //}
                    ProjectileRefresh();
                    UpdateEntities();
                    CheckPlayer();
                    Console.Write(sb);
                    Thread.Sleep(17);
                }
            }

            void SetPlayerPosiotion()
            {
                int coord = player.roundedY * 64 + player.roundedX;
                char[] znaki = sb.ToString().ToCharArray();
                char lastchar = znaki[coord];
                sb.Replace(lastchar, 'X', coord, 1);
                sb.Replace(lastchar, 'Q', coord - 64, 1);
            }

            void ClearPlayerPosiotion()
            {
                sb.Replace('Q',' ');
                sb.Replace('X',' ');
                //int coord = player.roundedY * 64 + player.roundedX;
                //char[] znaki = sb.ToString().ToCharArray();
                //char lastchar = znaki[coord];
                //sb.Replace();
            }

            void ShowLvlAndHP()
            {
                int zmienna = 1 * 64 + 1;
                sb.Remove(zmienna,55);
                string String1 = $"Poziom gracza: {player.lvl}                          Liczba żyć: {player.HealthPoints}";
                sb.Insert(zmienna, String1);
            }

            void ProjectileRefresh()
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    Projectile p = bullets[i];
                    int coord = p.roundedY * 64 + p.roundedX;
                    sb.Replace('*', ' ', coord, 1);

                    p.Update();


                    if (p.ColisionWithWall(render.rooms) || p.ColisionWithEntity(entities))
                    {
                        bullets.Remove(p);
                        i--;
                    }
                    else
                    {
                        coord = p.roundedY * 64 + p.roundedX;
                        sb.Replace(' ', '*', coord, 1);
                    }
                        
                }
            }

            void GenerateEntities()
            {

                int EntityCount = rnd.Next(2, 15);
                for (int i = 0; i < EntityCount; i++)
                {
                    int X = rnd.Next(2, 61);
                    int Y = rnd.Next(5, 26);
                    Entity entity = new Entity("easy", X, Y);
                    entities.Add(entity);
                }
                foreach (Entity entity in entities)
                {
                    int coord = entity.roundedY * 64 + entity.roundedX;
                    sb.Replace(chars[coord], entity.symbol, coord, 1);
                }
            }


            void UpdateEntities()
            {
                int RandomNumber = rnd.Next(101);

                for (int i = 0; i < entities.Count; i++)
                {
                    Entity entity = entities[i];
                    if (entity.ZeroHealth())
                    {
                        entities.Remove(entity);
                        int coord = entity.roundedY * 64 + entity.roundedX;
                        sb.Replace('E', ' ', coord, 1);
                        player.exp += 10;
                        i--;
                    }
                    if ((player.roundedX == entity.roundedX) || player.roundedY == entity.roundedY)
                    {
                        //if (RandomNumber < 21)
                        //{
                        //    Projectile bullet = new Projectile(entity.roundedX, entity.roundedY, entity.lookingdirection, "entity");
                        //    bullets.Add(bullet);
                        //}
                    }
                }
            }

            void CheckPlayer()
             {
                if (player.exp >= 50)
                {
                    player.lvl += 1;
                    player.exp = 0;
                    player.HealthPoints += 1;
                }
                if (player.HealthPoints == 0)
                    Environment.Exit(0);
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
                            player.roundedX = (int)Math.Round(player.X -= player.speed);
                            player.roundedY = (int)Math.Round(player.Y);
                            player.lookingdirection = "west";
                            if (player.ColisionWithWall(render.rooms)) { player.X += 1; player.roundedX += 1; }
                            break;
                        case ConsoleKey.W:
                            player.roundedX = (int)Math.Round(player.X);
                            player.roundedY = (int)Math.Round(player.Y -= player.speed);
                            player.lookingdirection = "north";
                            if (player.ColisionWithWall(render.rooms)) { player.Y += 1; player.roundedY += 1; }
                            break;
                        case ConsoleKey.S:
                            player.roundedX = (int)Math.Round(player.X);
                            player.roundedY = (int)Math.Round(player.Y += player.speed);
                            player.lookingdirection = "south";
                            if (player.ColisionWithWall(render.rooms)) { player.Y -= 1; player.roundedY -= 1; }
                            break;
                        case ConsoleKey.D:
                            player.roundedX = (int)Math.Round(player.X += player.speed);
                            player.roundedY = (int)Math.Round(player.Y);
                            player.lookingdirection = "east";
                            if (player.ColisionWithWall(render.rooms)) { player.X -= 1; player.roundedX -= 1; }
                            break;
                        case ConsoleKey.Spacebar:
                            Projectile bullet = new Projectile(player.roundedX, player.roundedY, player.lookingdirection, "player");
                            bullets.Add(bullet);
                            break;
                    }
                    Thread.Sleep(32);
                }
            }
        }
    }
}