using System.Collections.Generic;

namespace ClassLibrary
{
    public class InfoWindow
    {
        private static int windowX = 4;
        private static int windowY = 13;
        private static Queue<string> textLines = new Queue<string>();

        // Renders text into the information window on the user interface.
        public static void RenderInfoWindow(string text, int delay)
        {
            string textToRender = "";
            text = text.PadRight(62, ' ');
            if (textLines.Count == 3) textLines.Dequeue();
            textLines.Enqueue(text);

            foreach (string textLine in textLines)
            {
                textToRender += textLine.PadRight(62, ' ') + "`";
            }

            textToRender += "_".PadRight(62, ' ');

            GUI.TextWrite(windowX, windowY, textToRender, 25, 27, delay);
        }

        // Overload method to clear the information window on the user interface.
        public static void RenderInfoWindow(int delay)
        {
            textLines.Clear();
            for (int y = 3; y >= 0; y--)
            {
                for (int x = 61; x >= 0; x--) GUI.TextWrite(windowX + x, windowY + y, " ", 25, 27, delay);
            }
        }

    }
}
