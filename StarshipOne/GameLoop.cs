using ClassLibrary;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;


// using u3157923_9746_Assessment1;

// Advanced Game Programming (9746) Semester 1, 2020 - Robert Mair (u3157923@uni.canberra.edu.au)

// Assessment 2 – Prototype Game Project

// A prototype game built using a C# console application to demonstrate an understanding of the theoretical
// and practical aspects of the unit through the application of advanced game programming techniques. This
// work is focused on the implementation of a core game system to an advanced level of functionality using
// enemy AI including pathfinding.

namespace u3157923_9746_Assessment2
{
    [Guid("ED730B81-5564-4A0F-8E17-F5FE0D01001E")]

    public class GameLoop
    {
        // public static int level; // The level the player is up to.
        public static int playerMove; // Captures player move direction: No move = 0, Up = 1, Down = 2, Left = 3, Right = 4
        public static int playerStatus; // After each player move, the Player class returns a player status code (e.g. level completed, out of oxygen or energy).
        public static string playerText; // After each player move, the Player class may return some text to render in the Info Window.
        public static string npcText; // Used for any return string from the NPC Class to render to the Info Window.
        public static bool endProgram;
        public static int guiElement; // Used to hold a value of any 'GUI element number' the mouse cursor is positioned above.
        public static int levelLoopTimer;


        // Setup console to use ANSI/VT color codes (to set custom foreground/background colours.
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);

        // ********** MAIN PROGRAM LOOP **********

        public static void Main()
        {
            // Setup mouse input handler.
            var handle = GuiInput.GetStdHandle(GuiInput.STD_INPUT_HANDLE);
            int mode = 0;
            if (!GuiInput.GetConsoleMode(handle, ref mode)) throw new Win32Exception();
            mode |= GuiInput.ENABLE_MOUSE_INPUT;
            mode &= ~GuiInput.ENABLE_QUICK_EDIT_MODE;
            mode |= GuiInput.ENABLE_EXTENDED_FLAGS;
            if (!GuiInput.SetConsoleMode(handle, mode)) throw new Win32Exception();
            GuiInput.INPUT_RECORD record = new GuiInput.INPUT_RECORD();
            uint recordLen = 0;
            bool focusButton = false; // Used to swap button focus/unfocus state.

            InitialiseProgram.InitialiseProg(); // Initialise game environment for the first time.
            Level.StartLevel(0); // Initialise Level for the first time.

            // Game loop timer, used to render screen updates including player movement input.
            var autoEvent = new AutoResetEvent(false);
            var moveNpcs = new GameUpdate();
            var moveNpcsTimer = new Timer(moveNpcs.RenderGameUpdate,
                autoEvent, -1, -1);

            while (!endProgram)
            {
                // Get Player input to use each game loop.
                playerMove = PlayerControl.PlayerInput(handle, ref record, ref recordLen, ref focusButton);
                if (Level.level >= 1 && levelLoopTimer == 0)
                {
                    moveNpcsTimer.Change(1000, GUI.gameLoopCycle);
                    autoEvent.WaitOne();
                }

                if (GUI.pauseGame)
                {
                    moveNpcsTimer.Change(-1, -1);
                    // Unpauses game (need to check here as 'pause' locks out the RenderGAmeUpdate loop below, including the PlayerControl class).
                    if (GUI.pauseGame && playerMove == (int)PlayerEvent.ButtonClick && guiElement - 1 == 5)
                    {
                        GUI.pauseGame = false;
                        moveNpcsTimer.Change(0, GUI.gameLoopCycle);
                    }
                }
            }
        }

        class GameUpdate
        {
            public void RenderGameUpdate(Object stateInfo)
            {

                AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;

                if (!GUI.pauseGame)
                {
                    levelLoopTimer += 1;
                    playerText = "";

                    switch (playerMove)
                    {
                        case (int)PlayerEvent.NoEvent:
                            break;
                        case (int)PlayerEvent.MoveUp:
                            var values = Player.PlayerMove(0, -1);
                            playerText = values.Item1;
                            playerStatus = values.Item2;
                            break;
                        case (int)PlayerEvent.MoveDown:
                            values = Player.PlayerMove(0, 1);
                            playerText = values.Item1;
                            playerStatus = values.Item2;
                            break;
                        case (int)PlayerEvent.MoveLeft:
                            values = Player.PlayerMove(-1, 0);
                            playerText = values.Item1;
                            playerStatus = values.Item2;
                            break;
                        case (int)PlayerEvent.MoveRight:
                            values = Player.PlayerMove(1, 0);
                            playerText = values.Item1;
                            playerStatus = values.Item2;
                            break;
                        case (int)PlayerEvent.ButtonFocus: // Unfocus a button when it has focus but mouse has now moved off but onto another button.
                            MenuButtons.ButtonList[MenuButtons.buttonNumber].UnFocus();
                            break;
                        case (int)PlayerEvent.ButtonUnfocus:  // Focus a button when the mouse has moved over (unfocuses any previously focused button).
                            if (MenuButtons.buttonNumber >= 0)
                                MenuButtons.ButtonList[MenuButtons.buttonNumber].UnFocus();
                            if (guiElement > 0)
                            {
                                MenuButtons.ButtonList[guiElement - 1].Focus();
                                MenuButtons.buttonNumber = guiElement - 1;
                            }
                            break;
                        case (int)PlayerEvent.ButtonClick: // Trigger button event if button has focus and mouse is clicked.
                            MenuButtons.ButtonList[guiElement - 1].Action();
                            break;
                        case (int)PlayerEvent.ButtonAllUnfocus: // Unfocus all any focussed button when mouse is not above any button.
                            if (MenuButtons.buttonNumber >= 0)
                                MenuButtons.ButtonList[MenuButtons.buttonNumber].UnFocus();
                            MenuButtons.buttonNumber = -1;
                            break;

                        case (int)PlayerEvent.PlaceWall:
                            Grid.NodeGrid[PlayerControl.playAreaY][PlayerControl.playAreaX] =
                                NodeContentCollection.CreateNodeInstance('W');
                            Grid.RenderNode(Grid.GridXOrg, Grid.GridYOrg, PlayerControl.playAreaX, PlayerControl.playAreaY);
                            Player.wall--;
                            break;
                        case (int)PlayerEvent.RemoveWall:
                            Grid.NodeGrid[PlayerControl.playAreaY][PlayerControl.playAreaX] =
                                NodeContentCollection.CreateNodeInstance('.');
                            Grid.RenderNode(Grid.GridXOrg, Grid.GridYOrg, PlayerControl.playAreaX, PlayerControl.playAreaY);
                            Player.removeWall--;
                            break;
                    }

                    switch (playerStatus)
                    {
                        case 0: // Player has completed the level, go to the next level.
                            Level.level += 1;
                            playerStatus = -1;
                            levelLoopTimer = 0;
                            Level.StartLevel(Level.level);
                            Player.PlayerStartLevel(Level.Levels[Level.level].PlayerXOrg,
                                Level.Levels[Level.level].PlayerYOrg);


                            // Checks if player has finished the game, if so it resets the levels so the player can start again at the beginning.
                            if (Level.Levels[Level.level].LevelName == Level.finishedGame)
                            {
                                GUI.pauseGame = true;
                                playerStatus = -1;
                                levelLoopTimer = 0;
                                playerText = "";
                                Player.PlayerReset();
                                // Reset game levels.
                                Level.Levels.Clear();
                                // Create game levels.
                                Level.LevelCollection();
                                Level.level = 0;
                            }

                            break;
                        case 1:
                            // Player has run out of oxygen.
                            Sound.lowEnergyOxygen.Stop();
                            GUI.pauseGame = true;
                            playerStatus = -1;
                            levelLoopTimer = 0;
                            playerText = "";
                            Player.PlayerReset();
                            // Reset game levels.
                            Level.Levels.Clear();
                            // Create game levels.
                            Level.LevelCollection();
                            Level.level = Level.FindLevelByName(Level.permaDeath);
                            Level.StartLevel(Level.level);
                            break;
                        case 2:
                            // Player has run out of energy.
                            Sound.lowEnergyOxygen.Stop();
                            GUI.pauseGame = true;
                            playerStatus = -1;
                            levelLoopTimer = 0;
                            playerText = "";
                            Player.PlayerReset();
                            // Reset game levels.
                            Level.Levels.Clear();
                            // Create game levels.
                            Level.LevelCollection();
                            Level.level = Level.FindLevelByName(Level.permaDeath);
                            Level.StartLevel(Level.level);
                            break;
                        case 3:
                            // Player has found the Information Terminal.
                            if (Sound.soundFxOn) Sound.infoTerminal.PlaySound();
                            Level.InfoTerminalFound(Level.level);
                            playerStatus = -1;
                            break;
                        case 4:
                            // Player has completed the game.
                            GUI.pauseGame = true;
                            playerStatus = -1;
                            levelLoopTimer = 0;
                            playerText = "";
                            Player.PlayerReset();
                            // Reset game levels.
                            Level.Levels.Clear();
                            // Create game levels.
                            Level.LevelCollection();
                            Level.level = Level.FindLevelByName(Level.finishedGame);
                            Level.StartLevel(Level.level);
                            break;
                    }

                    if (playerText.Length > 0) InfoWindow.RenderInfoWindow(playerText, 0);
                    playerMove = 0;

                    npcText = "";
                    foreach (NpcBase npc in Level.Levels[Level.level].LevelNpcs)
                    {
                        if (levelLoopTimer > npc.NpcStartDelay &&
                            levelLoopTimer % npc.NpcMoveSpeed == 0) // && npc.NpcPwrLvl > 0)
                        {
                            if (npc.NpcX == Player.playerX && npc.NpcY == Player.playerY)
                            {
                                npcText = npc.NpcFoundPlayer();
                                if (npcText.Length > 0) InfoWindow.RenderInfoWindow(npcText, 0);
                            }
                            else
                            {
                                npc.NpcMoveDecision(Player.playerX, Player.playerY);
                            }
                            if (npcText.Length > 0) InfoWindow.RenderInfoWindow(npcText, 0);
                        }
                    }
                }

                // Update Player status bars.
                GUI.UpdateStatBar(80, 13, Player.playerOxyMax / Player.playerStatDiv, Player.playerOxyLvl / Player.playerStatDiv, Player.playerOxyWar / Player.playerStatDiv); // Oxygen.
                GUI.UpdateStatBar(80, 14, Player.playerPwrMax / Player.playerStatDiv, Player.playerPwrLvl / Player.playerStatDiv, Player.playerPwrWar / Player.playerStatDiv); // Power.
                GUI.UpdateStatBar(80, 30, 20, Player.wall, 20); // Number of 'walls' the Player has in inventory.
                GUI.UpdateStatBar(80, 32, 20, Player.removeWall, 20); // Number of 'walls removers' the Player has in inventory.

                // Player low oxygen/power warning sound.
                if (Player.playerOxyLvl <= Player.playerOxyWar && Player.warningAlarm == false || Player.playerPwrLvl <= Player.playerPwrWar && Player.warningAlarm == false)
                {
                    Sound.lowEnergyOxygen.SetVolume(0.5f);
                    if (Sound.soundFxOn) Sound.lowEnergyOxygen.PlaySound();
                    Player.warningAlarm = true;
                }
                else if (Player.playerOxyLvl > Player.playerOxyWar && Player.playerPwrLvl > Player.playerPwrWar)
                {
                    Sound.lowEnergyOxygen.Stop();
                    Player.warningAlarm = false;
                }

                GUI.TextWrite(5, 20, levelLoopTimer.ToString(), 25, 27, 0);

                autoEvent.Set();

            }
        }
    }
}
