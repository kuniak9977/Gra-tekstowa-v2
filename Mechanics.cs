using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gra_tekstowa_v2.Entity;

namespace Gra_tekstowa_v2
{
    public class Mechanics
    {
        Render render = new Render();
        Player player = new Player();
        List<Projectile> bullets = new List<Projectile>();
        List<Entity> entities = new List<Entity>();
        Random rnd = new Random();

        StringBuilder sb = new StringBuilder();
        
        string difficulty = "easy";
        char[] chars;

        ConsoleKeyInfo cki;
        ConsoleKey keyPressed;

        public Mechanics() 
        {
            Console.CursorVisible = false;
            render.LoadRoom();
            sb = render.GetStringBuilder();
            chars = sb.ToString().ToCharArray();
            GenerateEntities();
        }

        public void ScreenRefresh()
        {
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                ShowLvlAndHP();
                ClearPlayerPosiotion();
                SetPlayerPosiotion();
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
            sb.Replace('Q', ' ');
            sb.Replace('X', ' ');
        }

        void ShowLvlAndHP()
        {
            int zmienna = 1 * 64 + 1;
            sb.Remove(zmienna, 55);
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
                if (p.ColisonWithPlayer(player))
                {
                    bullets.Remove(p);
                    i--;
                    player.HealthPoints -= 1;
                }
                else if (p.ColisionWithWall(render.rooms) || p.ColisionWithEntity(entities))
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
                if (player.roundedX == entity.roundedX)
                {
                    if (player.roundedY >= entity.roundedY)
                        entity.lookingdirection = "south";
                    else
                        entity.lookingdirection = "north";
                }
                if (player.roundedY == entity.roundedY)
                {
                    if (player.roundedX >= entity.roundedX)
                        entity.lookingdirection = "east";
                    else
                        entity.lookingdirection = "west";
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

        public void EntityShooting()
        {
            while (true)
            {
                foreach (Entity entity in entities)
                {
                    if (SameLine(entity))
                    {
                        Projectile bullet = new Projectile(entity.roundedX, entity.roundedY, entity.lookingdirection, "entity");
                        bullets.Add(bullet);
                    }
                }
                Thread.Sleep(500);
            }
        }

        bool SameLine(Entity entity)
        {
            return (player.roundedX == entity.roundedX) || (player.roundedY == entity.roundedY);
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

        public void PlayerAction()
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
        public void RunMainMenu()
        {
            string[] options = { "Graj", "O grze", "Ustawienia", "Wyjdź" };
            string prompt = "Niewdzięczna gra";
            Menu MainMenu = new Menu(options, prompt);
            int ChoosenOption = MainMenu.Run();
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
            Console.Write("Aby kontynuować wciśnij ENTER");
        }
    }
}
