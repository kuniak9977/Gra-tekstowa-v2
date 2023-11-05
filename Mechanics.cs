using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Gra_tekstowa_v2.Entity;
using static Gra_tekstowa_v2.Font;

namespace Gra_tekstowa_v2
{
    public class Mechanics
    {
        Render render;
        Player player;
        List<Projectile> bullets;
        List<Entity> entities;
        Random rnd = new Random();

        StringBuilder sb = new StringBuilder();
        
        string difficulty;
        char[] chars;
        char[,] chars2;
        int entityshootspeed;
        int maxEntity, minEntity;
        int playerlocation = 0;
        public volatile bool isPaused = false;
        public bool exitGenerated = false;
        public bool playerIsDead = false;

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
            render = new Render(difficulty);
            player = new Player();
            bullets = new List<Projectile>();
            entities = new List<Entity>();
            LoadRoom(playerlocation);
            
            GenerateEntities();
            ScreeThread = new Thread(ScreenRefresh);
            PlayerThread = new Thread(PlayerAction);
            EntityShootThread = new Thread(EntityShooting);
            ScreeThread.Start();
            PlayerThread.Start();
            EntityShootThread.Start();
        }

        void LoadRoom(int location)
        {
            
            int rows = 27;
            int col = 64;
            chars = new char[rows * col];
            int cursor = 0;
            char[,] thing = (char[,])render.map.RoomList[location].roomchar.Clone();
            chars2 = (char[,])render.map.RoomList[location].roomchar.Clone();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    chars[cursor++] = thing[i,j];
                }
            }
            render.LoadRoom(location);
            //sb.Clear();
            sb = render.GetStringBuilder();
        }


        public void Resume()
        {
            isPaused = false;

            player = new Player();
            bullets = new List<Projectile>();
            entities = new List<Entity>();
            render = new Render(difficulty);
            playerlocation = 0;
            LoadRoom(playerlocation);
            GenerateEntities();
        }


        public void Stop()
        {
            isPaused = true;
        }

        public void ScreenRefresh()
        {
            while (true)
            {
                if (!isPaused)
                {
                    Console.SetCursorPosition(0, 0);
                    EnterToNeighbourRoom();
                    OpenDoor();
                    ShowLvlAndHP();
                    ClearPlayerPosiotion();
                    ProjectileRefresh();
                    UpdateEntities();
                    CheckPlayer();
                    ExitFromMaze();
                    SetPlayerPosiotion();
                    
                    if (CheckForAllroomsCleard())
                        GenerateExit();
                    Console.Write(sb);
                }
                Thread.Sleep(32);
            }
        }

        public bool CheckForAllroomsCleard()
        {
            int yes = 0;
            foreach (var r in render.map.RoomList)
            {
                if (r.RoomIsClear)
                    yes++;
            }
            return yes == render.map.size;
        }
        void ExitFromMaze()
        {
            int playerCoord = player.roundedY * 64 + player.roundedX;
            char[] stringBuilder = sb.ToString().ToCharArray();
            char znak = stringBuilder[playerCoord];
            if (znak == '▓')
                exitGenerated = true;

        }
        void GenerateExit()
        {
            int Exitgenerated = rnd.Next(0, render.map.size + 1);
            string[] exit = { "▒▒▒▒", "▒▓▓▒", "▒▓▓▒", "▒▒▒▒" };
            int[] coord = { 13 * 64 + 30, 14 * 64 + 30, 15 * 64 + 30, 16 * 64 + 30 };
            if (playerlocation == Exitgenerated)
                for (int i = 0; i < 4; i++)
                 {
                    sb.Remove(coord[i], 4);
                    sb.Insert(coord[i], exit[i]);
                }
        }

        bool CheckRoomStatus()
        {
            if (entities.Count == 0)
                {
                    render.map.RoomList[playerlocation].RoomIsClear = true;
                    return true;
                }
            return false;
                
        }

        void OpenDoor()
        {
            
            if (CheckRoomStatus())
            {
                
                char[] tab = render.map.RoomList[playerlocation].direction;
                int coord;
                for (int i = 0; i < tab.Length; i++)
                {
                    char c = tab[i];
                    if (c != ' ')
                        switch (i)
                        {
                            case 0:
                                coord = 3 * 64 + 30;
                                sb.Replace('█', '0', coord, 4);
                                for (int j = 30; j <= 34; j++)
                                    chars2[3, j] = '0';
                                break;
                            case 1:
                                coord = 26 * 64 + 30;
                                sb.Replace('█', '0', coord, 4);
                                for (int j = 30; j <= 34; j++)
                                    chars2[26, j] = '0';
                                break;
                            case 2:
                                for (int j = 13; j <= 16; j++)
                                {
                                    coord = j * 64 + 63;
                                    sb.Replace('█', '0', coord, 1);
                                    chars2[j, 63] = '0';
                                }
                                break;
                            case 3:
                                for (int j = 13; j <= 16; j++)
                                {
                                    coord = j * 64 + 0;
                                    sb.Replace('█', '0', coord, 1);
                                    chars2[j, 0] = '0';
                                }
                                break;
                        }
                }
                
            }
        }

        void EnterToNeighbourRoom()
        {
            if (player.EnterTheDoor(chars2))
            {
                int currentPlayerLocation = playerlocation;
                switch (player.lookingdirection)
                {
                    case "north":
                        playerlocation = render.map.RoomList[currentPlayerLocation].direction[0];
                        player.X = 32;
                        player.roundedX = 32;
                        player.Y = 25;
                        player.roundedY = 25;
                        break;
                    case "south":
                        playerlocation = render.map.RoomList[currentPlayerLocation].direction[1];
                        player.X = 32;
                        player.roundedX = 32;
                        player.Y = 5;
                        player.roundedY = 5;
                        break;
                    case "east":
                        playerlocation = render.map.RoomList[currentPlayerLocation].direction[2];
                        player.X = 2;
                        player.roundedX = 2;
                        player.Y = 14;
                        player.roundedY = 14;
                        break;
                    case "west":
                        playerlocation = render.map.RoomList[currentPlayerLocation].direction[3];
                        player.X = 62;
                        player.roundedX = 62;
                        player.Y = 14;
                        player.roundedY = 14;
                        break;
                }
                sb.Clear();
                LoadRoom(playerlocation);
                GenerateEntities();
                ShowLvlAndHP();
                ClearPlayerPosiotion();
                ProjectileRefresh();
                UpdateEntities();
                CheckPlayer();
                SetPlayerPosiotion();
                Console.Clear();
            }
        }

        void SetPlayerPosiotion()
        {
            int coord = player.roundedY * 64 + player.roundedX;
            char[] znaki = sb.ToString().ToCharArray();
            char lastchar = znaki[coord];
            player.ColisionWithEntity(entities);
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
                else if (p.ColisionWithWall(chars2) || p.ColisionWithEntity(entities))
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
                int coord = entity.roundedY * 64 + entity.roundedX;
                char[] chars = sb.ToString().ToCharArray();
                if (chars[coord] == ' ' && entities.Contains(entity))
                    sb.Replace(' ', 'E', coord, 1);
                if (entity.ZeroHealth())
                {
                    entities.Remove(entity);
                    //int coord = entity.roundedY * 64 + entity.roundedX;
                    
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
            if (render.map.RoomList[playerlocation].RoomIsClear == false)
            {
                int EntityCount = rnd.Next(minEntity, maxEntity);
                for (int i = 0; i < EntityCount; i++)
                {
                spawnAgain:
                    int X = rnd.Next(2, 61);
                    int Y = rnd.Next(5, 26);
                    int coord = (Y * 64) + X;
                    if (chars[coord] == '+')
                    {
                        Entity entity = new Entity(difficulty, X, Y);
                        entities.Add(entity);
                    }
                    else
                        goto spawnAgain;
                }
                foreach (Entity entity in entities)
                {
                    int coord = entity.roundedY * 64 + entity.roundedX;
                    sb.Replace(chars[coord], entity.symbol, coord, 1);
                }
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
                playerIsDead = true;
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
                            if (player.ColisionWithWall(chars2)) { player.X += 1; player.roundedX += 1; }
                            break;
                        case ConsoleKey.W:
                            player.roundedX = (int)Math.Round(player.X);
                            player.roundedY = (int)Math.Round(player.Y -= player.speed);
                            player.lookingdirection = "north";
                            if (player.ColisionWithWall(chars2)) { player.Y += 1; player.roundedY += 1; }
                            break;
                        case ConsoleKey.S:
                            player.roundedX = (int)Math.Round(player.X);
                            player.roundedY = (int)Math.Round(player.Y += player.speed);
                            player.lookingdirection = "south";
                            if (player.ColisionWithWall(chars2)) { player.Y -= 1; player.roundedY -= 1; }
                            break;
                        case ConsoleKey.D:
                            player.roundedX = (int)Math.Round(player.X += player.speed);
                            player.roundedY = (int)Math.Round(player.Y);
                            player.lookingdirection = "east";
                            if (player.ColisionWithWall(chars2)) { player.X -= 1; player.roundedX -= 1; }
                            break;
                        case ConsoleKey.Spacebar:
                            Projectile bullet = new Projectile(player.roundedX, player.roundedY, player.lookingdirection, "player");
                            bullets.Add(bullet);
                            break;
                        case ConsoleKey.Escape:
                            isPaused = true;
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
            Console.WriteLine(" W / S - góra/dół, ENTER - zatwierdź");
            Console.ReadKey();
            switch ( ChoosenOption )
            {
                case 0:
                    DifficultyLevelMenu();
                    if (PlayerThread is null)
                        Play();
                    else
                        Resume();
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
            Menu LevelMenu = new Menu(options, "Wybierz poziom trudności:");
            int ChoosenOption = LevelMenu.Run();
            switch (ChoosenOption)
            {
                case 0:
                    difficulty = "easy";
                    entityshootspeed = 1200;
                    maxEntity = 10;
                    minEntity = 4;
                    break;
                case 1:
                    difficulty = "medium";
                    entityshootspeed = 800;
                    maxEntity = 15;
                    minEntity = 8;
                    break;
                case 2:
                    difficulty = "hard";
                    entityshootspeed = 400;
                    maxEntity = 20;
                    minEntity = 12;
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

        public void PauseMenu()
        {
            
            Console.Clear();
            Stop();
            string Propmt = "PAUZA\n";
            string[] strings = { "Wróć do gry", "Powrót do menu", "Wyjdź z gry" };
            Menu PauseMenu = new Menu(strings, Propmt);
            int Choosenoption = PauseMenu.Run();

            switch (Choosenoption)
            {
                case 0:
                    isPaused = false;
                    //Resume();
                    break;
                case 1:
                    isPaused = true;
                    RunMainMenu();
                    break;
                case 2:
                    Environment.Exit(0); ;
                    break;
            }
        }
        public void DeadMenu()
        {
            playerIsDead = false;
            exitGenerated = false;
            string[] about = {
                "Niestety nie udało Ci się znaleźć wyjścia.",
                "Umarłeś i zostałeś zapomiany przez wszystkich.",
                "Tak kończą Ci którym się nie udało."
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
            Console.WriteLine(ToWrite);
            Console.ReadKey();
            playerIsDead = false;
            exitGenerated = false;

            //RunMainMenu();
        }
        public void ExitMenu()
        {
            Console.Clear();


            string[] option = { "Wróć do menu" };
            Menu EndText = new Menu(option, "Gratulacje! Udało Ci się znaleźć wyjście!");
            int opcja = EndText.Run();
            Console.ReadKey(true);
            playerIsDead = false;
            exitGenerated = false;

        }
    }
}
