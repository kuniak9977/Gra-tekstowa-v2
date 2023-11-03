using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
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
        
        string difficulty;
        char[] chars;
        int entityshootspeed;
        volatile bool isPaused = false;

        ConsoleKeyInfo cki;
        ConsoleKey keyPressed;
        Thread ScreeThread;
        Thread PlayerThread;
        Thread EntityShootThread;

        public Mechanics() 
        {
            
        }

        void Play()
        {
            Console.CursorVisible = false;
            render.LoadRoom();
            sb = render.GetStringBuilder();
            chars = sb.ToString().ToCharArray();
            GenerateEntities();
            ScreeThread = new Thread(ScreenRefresh);
            PlayerThread = new Thread(PlayerAction);
            EntityShootThread = new Thread(EntityShooting);
            ScreeThread.Start();
            PlayerThread.Start();
            EntityShootThread.Start();
        }

        void Pause()
        {
            isPaused = true;
            Stop();
        }

        void Resume()
        {
            isPaused = false;
            ScreeThread = new Thread(ScreenRefresh);
            PlayerThread = new Thread(PlayerAction);
            EntityShootThread = new Thread(EntityShooting);
            ScreeThread.Start();
            PlayerThread.Start();
            EntityShootThread.Start();
        }


        void Stop()
        {
            EntityShootThread.Join();
            PlayerThread.Join();
            ScreeThread.Join();
        }

        public void ScreenRefresh()
        {
            while (true)
            {
                if (!isPaused)
                {
                    Console.SetCursorPosition(0, 0);
                    ShowLvlAndHP();
                    ClearPlayerPosiotion();
                    SetPlayerPosiotion();
                    ProjectileRefresh();
                    UpdateEntities();
                    CheckPlayer();
                    Console.Write(sb);
                }
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
                Entity entity = new Entity(difficulty, X, Y);
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
                if (!isPaused)
                {
                    foreach (Entity entity in entities)
                    {
                        if (SameLine(entity))
                        {
                            Projectile bullet = new Projectile(entity.roundedX, entity.roundedY, entity.lookingdirection, "entity");
                            bullets.Add(bullet);
                        }
                    }
                }
                
                Thread.Sleep(entityshootspeed);
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
                if (!isPaused)
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
                        case ConsoleKey.Escape:
                            PauseMenu();
                            break;
                    }
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
            string ToWrite = "Aby kontynuować wciśnij dowolny przycisk";
            Console.SetCursorPosition((Console.WindowWidth / 2) - (ToWrite.Length / 2), Console.WindowHeight / 2);
            Console.Write(ToWrite);
            Console.ReadKey();
            switch ( ChoosenOption )
            {
                case 0:
                    DifficultyLevelMenu();
                    Play();
                    break;
                case 1:
                    AboutMenu();
                    break;
                case 2:
                    FontSizeMenu();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }
        }

        void DifficultyLevelMenu()
        {
            string[] options = { "Easy", "Medium", "Hard" };
            Menu LevelMenu = new Menu(options, "  ");
            int ChoosenOption = LevelMenu.Run();
            switch (ChoosenOption)
            {
                case 0:
                    difficulty = "easy";
                    entityshootspeed = 1200;
                    break;
                case 1:
                    difficulty = "medium";
                    entityshootspeed = 800;
                    break;
                case 2:
                    difficulty = "hard";
                    entityshootspeed = 400;
                    break;
            }
        }

        void AboutMenu()
        {
            string[] about = {
            "Gra polega na wyjściu z labiryntu pomieszczeń.",
            "Pojawisz się w pokoju startowym, po pokonaniu",
            "wszystkich przeciwnikó odblokują się",
            "przejścia do kolejnych pomieszczeń.",
            "Zadaniem jest dotrzeć do wyjścia i",
            "nie zginąć.","  ",
            "Podołasz wyzwaniu?"
            };
            int WidthSize = Console.WindowWidth;
            int HeightSize = Console.WindowHeight;
            int OptionQuantity = about.Length;
            int MiddlePointX = WidthSize / 2;
            int MiddlePointY = (HeightSize / 3) - (OptionQuantity / 2);

            Console.Clear();
            for (int i = 0; i < about.Length; i++)
            {
                string currentline = about[i];
                Console.SetCursorPosition(MiddlePointX - (currentline.Length / 2), MiddlePointY + 1 + i);
                Console.Write(currentline);
            }
            Console.WriteLine();
            string ToWrite = "Aby wrócić wciśnij dowolny przycisk";
            Console.SetCursorPosition((Console.WindowWidth / 2) - (ToWrite.Length / 2), Console.WindowHeight / 2);
            Console.ReadKey();

            RunMainMenu();

        }
        void FontSizeMenu()
        {
            string[] fonts = {"Mały (8)", "Średni (16)", "Duży (24)" , "Powrót do menu" };
            string prompt = "Rozmiar czcionki:";
            Menu FontMenu = new Menu(fonts, prompt);
            int Choosenoption = FontMenu.Run();
            switch (Choosenoption)
            {
                case 0:
                    Font.UstawCzcionke(8,16);
                    FontSizeMenu();
                    break;
                case 1:
                    Font.UstawCzcionke(16, 24);
                    FontSizeMenu();
                    break;
                case 2:
                    Font.UstawCzcionke(24, 32);
                    FontSizeMenu();
                    break;
                case 3:
                    RunMainMenu();
                    break;
            }
        }

        void PauseMenu()
        {
            Stop();
            Console.Clear();
            string Propmt = "PAUZA";
            string[] strings = { "Wróć do gry", "Powrót do menu", "Wyjdź z gry" };
            Menu PauseMenu = new Menu(strings, Propmt);
            int Choosenoption = PauseMenu.Run();

            switch (Choosenoption)
            {
                case 0:
                    Resume();
                    break;
                case 1:
                    RunMainMenu();
                    Stop();
                    break;
                case 2:
                    Environment.Exit(0); ;
                    break;
            }
        }
    }
}
