using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

// using Console = Colorful.Console;

namespace ClassLibrary
{
    public class GUI
    {

        public static int gameFps = 10; // Setup Frames Per Second (FPS) for game loop GUI updates.
        public static int gameLoopCycle = 1000 / gameFps; // In milliseconds, loop delay to move NPCs. Based on FPS (frames per second) i.e. 10 frames per second = 1000/10.
        public static bool pauseGame = false; // Used to 'pause' game when require by user or for other system level events.

        // Setup Console Window to required size for Program, and lock Console so it can't be resized by the User.
        protected const int MF_BYCOMMAND = 0x00000000;
        protected const int SC_MINIMIZE = 0xF020;
        protected const int SC_MAXIMIZE = 0xF030;
        protected const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        protected static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        protected static extern IntPtr GetConsoleWindow();

        public static void SetConsole(int ConX, int ConY)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);

            Console.SetWindowSize(ConX, ConY);
            Console.SetBufferSize(ConX, ConY);
            Console.CursorVisible = false;
            // Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
        }

        // Setup 'GUI element matrix' to hold information of what GUI element, such as a 'button' or 'text window' or 'game area', covers that
        // X,Y co-ordinate of the Console Window so when the mouse is moved or mouse button is clicked in that location an event can be triggered. 
        public static int[,] GuiElementGrid;
        public static void CreateGuiElementGrid(int conX, int conY)
        {
            GuiElementGrid = new int[conY, conX];
        }

        public static void ClearGuiElementGrid(int conX, int conY)
        {
            for (int row = 0; row < conY; row++)
            {
                for (int col = 0; col < conX; col++) GuiElementGrid[row, col] = 0;
            }
        }

        // Read contents of a text file and return it as a string (so it can to be rendered to Console Window).
        public static string FetchTextFile(string file)
        {
            string dataFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                                      "\\DataFiles\\" + file;
            if (File.Exists(dataFilePath))
            {
                try
                {
                    string text = File.ReadAllText(dataFilePath);
                    text = StripControlChars(text);
                    return text;
                }
                catch (Exception error)
                {
                    return error.Message;
                }
            }

            return "File Not Found: " + file; ;
        }

        // Clean up string to remove all unwanted characters.
        public static string CleanString(string text)
        {
            text = Regex.Replace(text, @"<[^>]+>|&nbsp;", "").Trim();
            return Regex.Replace(text, @"[^\u0020-\u007F]", String.Empty);
        }

        static string StripControlChars(string arg)
        {
            char[] arrForm = arg.ToCharArray();
            StringBuilder buffer = new StringBuilder(arg.Length); //This many chars at most

            foreach (char ch in arrForm)
                if (!Char.IsControl(ch)) buffer.Append(ch);//Only add to buffer if not a control char

            return buffer.ToString();
        }

        // Render contents of string to Console Window, starting at specified co-ordinates.
        public static void RenderTextToConsole(int x, int y, string text)
        {
            text = StripControlChars(text);

            Console.SetCursorPosition(x, y);
            bool special = false;
            foreach (char chr in text)
            {
                if (chr == '`')
                {
                    special = true;
                    continue;
                }

                if (special)
                {
                    special = false;
                    int choice = chr - 33;
                    switch (choice)
                    {
                        case int n when n >= 0 && n <= 26:
                            ColourPalette.FgColour(n);
                            break;
                        case int n when n >= 27 && n <= 31:
                            ColourPalette.BgColour(n);
                            break;
                        case 54:
                            y++;
                            Console.SetCursorPosition(x, y);
                            break;
                        case 55:
                            Console.Clear();
                            break;
                    }

                    continue;
                }
                Console.Write(chr);
            }
        }

        // Simple text 
        public static void TextWrite(int x, int y, string text, int fgColour, int bgColour, int delay)
        {
            // text = CleanString(text);

            ColourPalette.FgColour(fgColour);
            ColourPalette.BgColour(bgColour);
            Console.SetCursorPosition(x, y);
            foreach (char chr in text)
            {
                if (chr == '`') // Control character used to create a 'carriage return' when writing text to console.
                {
                    y++;
                    Console.SetCursorPosition(x, y);
                    continue;
                }
                Console.Write(chr);
                // if (delay > 0) Thread.Sleep(delay);
            }
        }

        // Simple char 
        public static void CharWrite(int x, int y, char chr, int fgColour, int bgColour)
        {
            ColourPalette.FgColour(fgColour);
            ColourPalette.BgColour(bgColour);
            Console.SetCursorPosition(x, y);
            Console.Write(chr);
        }

        // Updates horizontal 'status/statistics' level bars.
        public static void UpdateStatBar(int x, int y, int max, int level, int warningLevel)
        {

            int fgColourOK = 10; // Colour = Green for level OK.
            int fgColourWarn = 15; // Colour = Orange for level low warning.
            int bgColour = 27;

            for (int i = 0; i < level; i++)
            {
                int fgColour = level > warningLevel ? fgColourOK : fgColourWarn;
                CharWrite(x + i, y, '■', fgColour, bgColour);
            }

            for (int i = level; i < max; i++)
            {
                CharWrite(x + i, y, '-', fgColourWarn, bgColour);
            }

        }
    }
}