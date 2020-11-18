namespace ClassLibrary
{
    public class Player
    {
        private const char PLAYER_CHAR = '@';
        public static int playerX;
        public static int playerY;
        public static int playerOxyMax = 1000;
        public static int playerOxyLvl = 1000;
        public static int playerOxyWar = 250;
        public static int playerPwrMax = 1000;
        public static int playerPwrLvl = 1000;
        public static int playerPwrWar = 250;
        public static int playerStatDiv = 50; // Divides oxygen and power values to align with stat bars on GUI as they only have 20 marks (so 1000/50=20).

        private static int _playerZnC12CellsLvl;
        private static int _playerZnC12CellsMax = 20;
        private static bool _endLevel;
        private static bool _nextLevel;
        private static bool _endGame = false;
        private static int _playerStatus = -1; // Return code to main program loop (e.g. level completed, out of oxygen or energy).
        private static string _playerText;
        public static bool warningAlarm;

        // Player 'collectibles' to start the game.
        private static int _doorKeys;
        public static int wall = 10; // Inventory of 'temporary walls' the player can place in the play area to defend against NPCs.
        public static int removeWall = 10; // Inventory of ability to remove 'temporary walls'. Handy in case the player has blocked an exit.
        private const int MAX_COLLECTIBLE = 20;

        public static void PlayerReset()
        {
            playerOxyLvl = playerOxyMax;
            playerPwrLvl = playerPwrMax;
            _playerZnC12CellsLvl = 0;
            _endLevel = false;
            _nextLevel = false;
            _playerStatus = -1;
            _doorKeys = 0;
            wall = 10;
        }
        public static void PlayerStartLevel(int playerXOrg, int playerYOrg)
        {
            _endLevel = false;
            _nextLevel = false;
            playerX = playerXOrg;
            playerY = playerYOrg;
            _playerZnC12CellsLvl = 0;
            _playerStatus = -1;
            GUI.UpdateStatBar(80, 13, playerOxyMax / playerStatDiv, playerOxyLvl / playerStatDiv, playerOxyWar / playerStatDiv);
            GUI.UpdateStatBar(80, 14, playerPwrMax / playerStatDiv, playerPwrLvl / playerStatDiv, playerPwrWar / playerStatDiv);
            GUI.UpdateStatBar(80, 16, 20, _playerZnC12CellsLvl, 0);
            GUI.CharWrite(Grid.GridXOrg + playerX, Grid.GridYOrg + playerY, PLAYER_CHAR, 25, 27);
        }

        public static (string, int) PlayerMove(int moveX, int moveY)
        {
            _playerStatus = -1;
            _playerText = "";

            char nextNode = CheckNodeDirection(playerX, playerY, moveX, moveY);
            if (playerX + moveX > 0 &&
                playerX + moveX < Grid.GridXSize &&
                playerY + moveY > 0 &&
                playerY + moveY < Grid.GridYSize &&
                NodeContentCollection.NodeContents[Grid.NodeGrid[playerY + moveY][playerX + moveX]].Walkable &&
                nextNode != '`' || nextNode == '*')
            {

                if (nextNode == '*')
                {
                    FoundLockedDoor(moveX, moveY);
                    return (_playerText, _playerStatus);
                }

                Grid.RenderNode(Grid.GridXOrg, Grid.GridYOrg, playerX, playerY);  // Renders grid node content for where player is moving from.
                playerX += moveX;
                playerY += moveY;
                GUI.CharWrite(Grid.GridXOrg + playerX, Grid.GridYOrg + playerY, PLAYER_CHAR, 25, 27); // Renders player in new location.

                PlayerStep(); // Updates oxygen and energy level of player for each move they make.

                // Checks for what the player is going to find on the next node they step onto, and what to do if they find these items.
                if (nextNode == 'z') FoundZnCl2Cell();
                if (nextNode == 'e') FoundEnergyCell();
                if (nextNode == 'o') FoundOxygenCell();
                if (nextNode == '-' || nextNode == '|') FoundForceField();
                if (nextNode == 'A' || nextNode == 'L') FoundLevelExit(nextNode);
                if (nextNode == '~')
                {
                    GUI.pauseGame = true;
                    FoundIce(moveX, moveY);
                }

                if (nextNode == '?') _playerStatus = 3;
                if (nextNode == 'k') FoundKey();
                if (nextNode == 'w') FoundWall();
                if (nextNode == 'r') FoundWallRemover();

            }

            if (_nextLevel) // Player has completed the level, time to go to the next level.
            {
                _playerStatus = 0;
                _nextLevel = false;
            }

            if (_endGame) // Player has completed the game.
            {
                _playerStatus = 4;
                _nextLevel = false;
                _endGame = false;
            }

            if (playerOxyLvl <= 0) _playerStatus = 1; // Player is out of oxygen, permadeath.
            if (playerPwrLvl <= 0) _playerStatus = 2; // Player is out of energy, permadeath.

            return (_playerText, _playerStatus);
        }

        // Nodes are 'directional' e.g. you can only step into a node from certain directions, this function checks to ensure the direction
        // the node is being entered from is allowable.
        public static char CheckNodeDirection(int originX, int originY, int moveX, int moveY)
        {
            if (NodeContentCollection.NodeContents[Grid.NodeGrid[originY + moveY][originX + moveX]].Up == moveY ||
                NodeContentCollection.NodeContents[Grid.NodeGrid[originY + moveY][originX + moveX]].Down == moveY ||
                NodeContentCollection.NodeContents[Grid.NodeGrid[originY + moveY][originX + moveX]].Left == moveX ||
                NodeContentCollection.NodeContents[Grid.NodeGrid[originY + moveY][originX + moveX]].Right == moveX
            )
            {
                return NodeContentCollection.NodeContents[Grid.NodeGrid[originY + moveY][originX + moveX]].Character;
            }

            return '`';
        }

        private static void PlayerStep()
        {
            Sound.playerStep.SetVolume(0.5f);
            if (Sound.soundFxOn) Sound.playerStep.PlaySound();
            // Every move player makes changes oxygen level.
            playerOxyLvl += NodeContentCollection.NodeContents[Grid.NodeGrid[playerY][playerX]].Oxygen;
            if (playerOxyLvl < 0) playerOxyLvl = 0;
            if (playerOxyLvl > playerOxyMax) playerOxyLvl = playerOxyMax;

            // Every move player makes changes space suite power level.
            playerPwrLvl += NodeContentCollection.NodeContents[Grid.NodeGrid[playerY][playerX]].Energy;
            if (playerPwrLvl < 0) playerPwrLvl = 0;
            if (playerPwrLvl > playerPwrMax) playerPwrLvl = playerOxyMax;


        }

        private static void FoundZnCl2Cell()
        {

            if (_playerZnC12CellsLvl < _playerZnC12CellsMax)
            {
                if (Sound.soundFxOn) Sound.collectible.PlaySound();
                _playerZnC12CellsLvl += 1;
                GUI.UpdateStatBar(80, 16, 20, _playerZnC12CellsLvl, 0);
                _playerText = _playerZnC12CellsLvl == 1 ? "Well done, your first ZnCl2 cell!" : "That's ZnCl2 cell number " + _playerZnC12CellsLvl + ", " + (_playerZnC12CellsMax - _playerZnC12CellsLvl) + " to go.";

            }

            if (_playerZnC12CellsLvl == _playerZnC12CellsMax && _endLevel == false)
            {
                if (Sound.soundFxOn) Sound.airLockOpen.PlaySound();
                _playerText = "All ZnCl2 cells collected, airlock has opened.";
                _endLevel = true;

            }
            Grid.NodeGrid[playerY][playerX] = NodeContentCollection.CreateNodeInstance('.');
        }

        private static void FoundOxygenCell()
        {
            if (Sound.soundFxOn) Sound.collectible.PlaySound();
            _playerText = playerOxyLvl == playerOxyMax
                ? "An oxygen cell, maximum oxygen level restored!"
                : "An oxygen cell, you have some more breathing space!";
            Grid.NodeGrid[playerY][playerX] = NodeContentCollection.CreateNodeInstance('.');
        }

        private static void FoundEnergyCell()
        {
            if (Sound.soundFxOn) Sound.collectible.PlaySound();
            _playerText = playerPwrLvl == playerPwrMax
                ? "An energy cell, maximum suite energy restored!"
                : "An energy cell, feeling the power?";
            Grid.NodeGrid[playerY][playerX] = NodeContentCollection.CreateNodeInstance('.');
        }

        private static void FoundForceField()
        {
            if (Sound.soundFxOn) Sound.forceField.PlaySound();
            _playerText = playerPwrLvl == 0
                ? "A force field, it has sapped all your suite power!"
                : "A force field, it has sapped some of your suite power!";
        }

        private static void FoundLevelExit(char nextNode)
        {
            if (_endLevel)
            {
                if (Sound.soundFxOn) Sound.goThroughAirLock.PlaySound();
                _nextLevel = true;
                if (Level.level == 3) _endGame = true;
            }
            else
            {
                if (Sound.soundFxOn) Sound.doorLocked.PlaySound();
            }

            string exitType = nextNode == 'A' ? "AIRLOCK" : "LIFT";
            _playerText = _endLevel
                ? "The " + exitType + " is now unlocked, get ready for the next level!"
                : "The " + exitType + " is locked, you haven't found all the ZnCl2 cells.";
        }

        private static void FoundIce(int moveX, int moveY)
        {
            _playerText = "Sliding on ice...";
            GUI.pauseGame = false;
            PlayerMove(moveX, moveY);
        }

        private static void FoundKey()
        {
            if (Sound.soundFxOn) Sound.collectible.PlaySound();
            if (_doorKeys < MAX_COLLECTIBLE) _doorKeys++;
            _playerText = "You have found a door key, well done.";
            GUI.UpdateStatBar(80, 23, MAX_COLLECTIBLE, _doorKeys, 0);
            Grid.NodeGrid[playerY][playerX] = NodeContentCollection.CreateNodeInstance('.');
        }

        private static void FoundLockedDoor(int moveX, int moveY)
        {
            if (_doorKeys > 0)
            {
                _playerText = "You have unlocked the door.";
                _doorKeys--;
                Grid.NodeGrid[playerY + moveY][playerX + moveX] = NodeContentCollection.CreateNodeInstance('+');
                Grid.RenderNode(Grid.GridXOrg, Grid.GridYOrg, playerX + moveX, playerY + moveY);
                GUI.UpdateStatBar(80, 23, MAX_COLLECTIBLE, _doorKeys, 0);
            }
            else
            {
                _playerText = "The door is locked, you need a key!";
            }
        }

        private static void FoundWall()
        {
            if (Sound.soundFxOn) Sound.collectible.PlaySound();
            if (wall < 20) wall++;
            Grid.NodeGrid[playerY][playerX] = NodeContentCollection.CreateNodeInstance('.');
        }

        private static void FoundWallRemover()
        {
            if (Sound.soundFxOn) Sound.collectible.PlaySound();
            if (removeWall < 20) removeWall++;
            Grid.NodeGrid[playerY][playerX] = NodeContentCollection.CreateNodeInstance('.');
        }

    }
}
