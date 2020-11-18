using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClassLibrary
{
    public class Level
    {
        public static string permaDeath = "Permanent Death!";
        public static string finishedGame = "Game Completed!";
        public static int level = 0;
        public int LevelNumber { get; set; }
        public string LevelName { get; set; }
        public string LevelFile { get; set; }
        public string LevelMusic { get; set; }
        public bool PlayerPlay { get; set; }
        public int PlayerXOrg { get; set; }
        public int PlayerYOrg { get; set; }
        public bool PermaDeath { get; set; }
        public bool FinalLevel { get; set; }
        public List<string> LevelText { get; set; } // Used for text in the 'Info Window' when each level is started.
        public List<string> LevelInfoText { get; set; } // Used for text in the 'Info Terminal' for when the player uses this to gather information.

        public List<NpcBase> LevelNpcs { get; set; }

        public static List<Level> Levels = new List<Level>();

        // Constructor: for level class instances to be placed into the level collection.

        public Level(int levelNumber, string levelName, string levelFile, string levelMusic, bool playerPlay, int playerXOrg, int playerYOrg,
            bool permaDeath, bool finalLevel, List<string> levelText, List<string> levelInfoText, List<NpcBase> levelNpcs)
        {
            LevelNumber = levelNumber;
            LevelName = levelName;
            LevelFile = levelFile;
            LevelMusic = levelMusic;
            PlayerPlay = playerPlay;
            PlayerXOrg = playerXOrg;
            PlayerYOrg = playerYOrg;
            PermaDeath = permaDeath;
            FinalLevel = finalLevel;
            LevelText = levelText;
            LevelInfoText = levelInfoText;
            LevelNpcs = levelNpcs;
        }

        public static void LevelCollection()
        {
            Levels.Add(new Level(0, "", "00_0StartScreen.txt", "Underclocked (underunderclocked mix).wav",
                false, 0, 0,
                false, false,
                new List<string>
                    { "Press any key to start game (or level number to jump ahead)." },
                new List<string>
                    { "" },
                new List<NpcBase>()
            ));


            Levels.Add(new Level(0, "00_1 Hold: Aft", "00_1HoldAft.txt", "",
                true, 32, 9, false, false,
                   new List<string>
                   { "Contr.. to Riley ETO, Starship One ..alfunct..launched...",
                       "..check ..info. terminal [?]..for .. more..details......."},
                   new List<string>
                   {
                       "********** INFORMATION TERMINAL: YOU MUST READ THIS **********",
                       "Each level requires 20 'z' ZnC12 cells to be collect. Collect ",
                       "Oxygen 'o' and Energy 'e' cells to maintain your space suite! ",
                       //"Cells for your suite to stay alive."
                   },
                   new List<NpcBase>
                       {
                           new Homer('H',50,10,1000,1000,200,15, 27,GUI.gameFps,GUI.gameFps/1, GUI.gameFps/1),
                           new Homer('H',50,10,1000,1000,200,15, 27,GUI.gameFps*3,GUI.gameFps/2, GUI.gameFps/2)
                       }
                   ));

            // Player start x=2, y=9
            Levels.Add(new Level(1, "00_2 Hold: Mid", "00_2HoldMid.txt", "",
                true, 57, 9, false, false,
                 new List<string>
                 {
                     "You have made it to level 2, well done Riley...",
                     "..but can you survive and make it to the next one?"
                 },
                 new List<string>
                 {
                     "********** INFORMATION TERMINAL: YOU MUST READ THIS **********",
                     "There is ice '~' about, no wonder it is so cold. Keys 'k' are ",
                     "used to open locked doors '*'."
                 },
                 new List<NpcBase>
                 {
                     new Homer('H',57,9,1000,1000,200,15, 27,GUI.gameFps,GUI.gameFps/1, GUI.gameFps/1),
                     new Homer('H',57,9,1000,1000,200,15, 27,GUI.gameFps*3, GUI.gameFps/2, GUI.gameFps/2),
                     new Homer('H',57,9,1000,1000,200,15, 27,GUI.gameFps*5,GUI.gameFps/2, GUI.gameFps/2),
                     new Homer('H',57,9,1000,1000,200,15, 27,GUI.gameFps*8,GUI.gameFps/3, GUI.gameFps/3)
                 }
                 ));

            Levels.Add(new Level(2, "04 Level: Navigation_CIC (the final level)", "04_1NavigationCIC (the final level).txt", "",
                true, 17, 8, false, false,
                new List<string>
                { "You have made it to the Navigation_CIC level.",
                    "This is the last level to complete and",
                    "Starship One will be under your control!" },
                new List<string> { "" },
                new List<NpcBase>
                {
                    new Homer('H',27,10,1000,1000,200,15, 27,GUI.gameFps/2,GUI.gameFps/1, GUI.gameFps/1),
                    new Homer('H',45,12,1000,1000,200,15, 27,GUI.gameFps/2,GUI.gameFps/1, GUI.gameFps/1),
                    new Homer('H',17,8,1000,1000,200,15, 27,GUI.gameFps*5,GUI.gameFps/1, GUI.gameFps/1),
                    new Homer('H',17,8,1000,1000,200,15, 27,GUI.gameFps*8, GUI.gameFps/3, GUI.gameFps/3),
                    new Homer('H',17,8,1000,1000,200,15, 27,GUI.gameFps*11, GUI.gameFps/3, GUI.gameFps/3)

                }
            ));

            Levels.Add(new Level(3, "Game Completed!", "gameCompleted.txt", "",
                false, 0, 0, false, true,
                new List<string>
                { "Well done! You have regained control of Starship One.",
                    "You can now return to Earth as a hero who has saved",
                    "all humankind from a terrible fate! Awesome!" },
                new List<string> { "" },
                new List<NpcBase>()
            ));

            Levels.Add(new Level(4, permaDeath, "permaDeath.txt", "",
                false, 0, 0, true, false,
                new List<string>
                    { "You have suffered a fate worse than death.",
                        "Starship One will now travel to the Andromeda Galaxy.",
                        "All humankind will perish as Earths resources run out!" },
                new List<string> { "" },
                new List<NpcBase>()
                ));

        }

        public static void StartLevel(int lvl)
        {
            level = lvl;
            string text = GUI.FetchTextFile(Levels[level].LevelFile);
            string levelName = Levels[level].LevelName.PadRight(62, ' ');
            GUI.TextWrite(4, 18, levelName, 25, 27, 0);
            GUI.UpdateStatBar(80, 18, 20, Levels[level].LevelNumber, 0);

            if (Levels[level].LevelMusic != "")
            {
                Sound.music.Stop();
                Sound.music.ChangeSoundTo(new MemoryStream(File.ReadAllBytes(Sound.dataFilePath + Levels[level].LevelMusic)),
                true);
                Sound.music.PlaySound();
            }

            // If the level is not a 'static level' i.e. the player 'plays' this level, use this.
            if (Levels[level].PlayerPlay)
            {
                List<char> gridStartNodes = text.ToList();
                Grid.NodeGrid.Clear();
                Grid.GridStartNodes(gridStartNodes);
                // Setup and render the 'grid' (used for the game play area).
                Grid.CreateGrid();
                InfoWindow.RenderInfoWindow(0); // Clears Info Window.
                foreach (string line in Levels[level].LevelText)
                {
                    InfoWindow.RenderInfoWindow(line, 2);
                }
                Player.PlayerStartLevel(Levels[level].PlayerXOrg,
                    Levels[level].PlayerYOrg);
                if (level > 0) GUI.pauseGame = false;
            }

            // If the level is a 'static level' i.e. the start, permadeath, or game completed levels (in which the player does not 'play' the level), use this.
            if (!Levels[level].PlayerPlay)
            {
                GUI.RenderTextToConsole(Grid.GridXOrg, Grid.GridYOrg, text);
                InfoWindow.RenderInfoWindow(0); // Clears Info Window.
                foreach (string line in Levels[level].LevelText)
                {
                    InfoWindow.RenderInfoWindow(line, 2);
                }

                ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                if (char.IsDigit(keyPressed.KeyChar))
                {
                    int keyNumber = int.Parse(keyPressed.KeyChar.ToString());
                    if (keyNumber > 0 && keyNumber < Levels.Count - 1)
                    {
                        level = keyNumber;
                        StartLevel(level);
                    }
                }
                else
                {
                    if (Levels[level].LevelName == permaDeath || Levels[level].LevelName == finishedGame)
                    {
                        level = 0;
                    }
                    else
                    {
                        level = 1;
                    }
                    StartLevel(level);
                }

            }

        }

        // When player encounters the 'information terminal' for a level, this renders the information in the 'info window'.
        public static void InfoTerminalFound(int level)
        {
            InfoWindow.RenderInfoWindow(0); // Clears Info Window.
            foreach (string line in Levels[level].LevelInfoText)
            {
                InfoWindow.RenderInfoWindow(line, 2);
            }
        }

        // Example: LINQ Query to find the 'PermaDeath' level data.
        // Then extract the level name from the LINQ query and use this
        // to find the IndexOf() the 'PermaDeath' level to render the
        // player death sequence.
        // P.S. LINQ is only used here for demonstration purposes, normally
        // would simply use IndexOf().
        public static int FindLevelByName(string levelName)
        {
            var findLevelLinq = (from s in Levels
                                 where s.LevelName == levelName
                                 select s).First();
            string levelNameFromLinq = findLevelLinq.LevelName;
            int index = Levels.IndexOf(Levels.First(i => i.LevelName == levelNameFromLinq));
            return index;
        }

    }

}
