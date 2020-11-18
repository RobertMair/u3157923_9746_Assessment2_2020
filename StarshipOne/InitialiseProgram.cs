using ClassLibrary;

namespace u3157923_9746_Assessment2
{
    public class InitialiseProgram
    {
        private const int ConX = 102; // Set width of Console Window.
        private const int ConY = 45; // Set height of Console Window.
        private static string _text; // Used to create a string to pass to screen rendering methods.
                                     // Default grid data set - the level data is loaded from a .txt file and stored in this
                                     // list then used to create the node object instances by correlating the chars from the
                                     // level .txt file with the GridNode Object instances.
        public static void InitialiseProg()
        {

            // Setup console configuration etc. required for Starship One.

            // Setup console to use ANSI/VT color codes (to set custom foreground/background colours.
            var colourHandle = GameLoop.GetStdHandle(-11);
            int colourMode;
            GameLoop.GetConsoleMode(colourHandle, out colourMode);
            GameLoop.SetConsoleMode(colourHandle, colourMode | 0x4);

            // Setup Grid variables etc. required for Starship One.
            Grid.DefaultNodeChar = '#'; // Default character to place in 'grid' at each 'node' when first creating the grid.
            Grid.CurrentNodeChar = '#';
            Grid.GridXOrg = 4; // Set X origin of 'grid' (area used for game play).
            Grid.GridYOrg = 21; // Set Y origin of 'grid' (area used for game play).

            Grid.GridXSize = 62; // Set maximum width size of 'grid'.
            Grid.GridYSize = 20; // Set maximum height size of 'grid'.

            // Call NodeList function to establish default node types (that are used in the 'grid') that can be used in the game..
            NodeList.SetNodes();

            // Set Console for Program.
            GUI.SetConsole(ConX, ConY);

            // Set GuiElement requirements for Program.
            GUI.CreateGuiElementGrid(ConX, ConY);

            // Setup 'border' x/y co-ordinates for setting up GUI Elements (only for development purposes).
            _text = GUI.FetchTextFile("border.txt");
            GUI.RenderTextToConsole(0, 0, _text);

            // Setup user interface design (background) for Starship One.
            _text = GUI.FetchTextFile("starshipOneBackground.txt");
            GUI.RenderTextToConsole(2, 2, _text);

            // Populate the list with objects of type 'node' and setup Buttons to select each node type.
            MenuButtons.CreateMenuButtons();
            MenuButtons.ButtonList.Clear();

            // Setup other Buttons required.
            MenuButtons.CreateMenuButtons();

            // Create game levels.
            Level.LevelCollection();

            // Set GameLoop variables.
            // GameLoop.level = 0;
            GameLoop.playerMove = 0;
            GameLoop.playerStatus = -1;
            GameLoop.playerText = "";
            GameLoop.endProgram = false;
            GameLoop.guiElement = 0;
            GameLoop.levelLoopTimer = 0;

        }
    }
}
