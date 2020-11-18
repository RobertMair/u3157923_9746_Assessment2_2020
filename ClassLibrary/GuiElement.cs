using System;

namespace ClassLibrary
{
    // *****  BUTTON CLASS *****
    // Used to setup Buttons to be used in the Console window by the program that can be clicked on by the user to capture user input/action event.

    // GuiElement class for user interface.
    public class GuiElement
    {
        public int OrgX { get; set; }
        public int OrgY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Label { get; set; }
        public int FgColour { get; set; }
        public int BgColour { get; set; }
        public int Number { get; set; }
        public bool EnableFocus { get; set; }

        private readonly IAction _iaction;

        // Constructor for 'Text Window' area on Console Window.
        public GuiElement(int orgX, int orgY, int width, int height, string label, int fgcolour,
            int bgcolour, int number)
        {

            OrgX = orgX;
            OrgY = orgY;
            Width = width;
            Height = height;
            Label = label;
            FgColour = fgcolour;
            BgColour = bgcolour;
            Number = number;

            ColourPalette.FgColour(FgColour);
            ColourPalette.BgColour(BgColour);

            Render();

        }

        // Constructor for a 'Button' area on Console Window.
        public GuiElement(int orgX, int orgY, int width, int height, string label, int fgcolour,
            int bgcolour, int number, bool enablefocus, IAction iaction)
        : this(orgX, orgY, width, height, label, fgcolour, bgcolour, number)
        {

            EnableFocus = enablefocus;
            _iaction = iaction;

            ColourPalette.FgColour(FgColour);
            ColourPalette.BgColour(BgColour);

            Render();
        }

        public void Render()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    GUI.GuiElementGrid[OrgY + y, OrgX + x] = Number;
                    Console.SetCursorPosition(OrgX + x, OrgY + y);
                    Console.Write(' ');
                }
            }

            int carriageReturn = 0;
            Console.SetCursorPosition(OrgX, OrgY);
            foreach (char chr in Label)
            {
                if (chr == '`')
                {
                    carriageReturn++;
                    Console.SetCursorPosition(OrgX, OrgY + carriageReturn);
                    continue;
                }
                Console.Write(chr);
            }

        }

#pragma warning disable IDE0060 // Remove unused parameter
        public void Action(params object[] list)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            _iaction.Action();
        }

        public void Focus()
        {
            if (EnableFocus)
            {
                ColourPalette.FgColour(BgColour - 27);
                ColourPalette.BgColour(FgColour + 27);
                Render();
            }
        }

        public void UnFocus()
        {
            if (EnableFocus)
            {
                ColourPalette.FgColour(FgColour);
                ColourPalette.BgColour(BgColour);
                Render();
            }
        }
    }

    public interface IAction
    {
        void Action(params object[] list);
    }

}