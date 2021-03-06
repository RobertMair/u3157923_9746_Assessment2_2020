﻿using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class ColourPalette

    {

        // Setup Console colour list, for use with FgColour() and BgColour() functions to change Console colours.
        public static List<string> colorList = ColorList();
        public static List<string> ColorList()
        {
            // ConsoleColor ENUM, place in list.
            ConsoleColor[] colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
            List<string> colorList = new List<string>();
            int count = 0;
            foreach (var color in colors)
            {
                colorList.Add(color.ToString());
                count += 1;
            }

            return colorList;
        }


        //// Setup custom colour palette for game levels.

        //public static int[,] palette =
        //{
        //    {0,0,0},        // Black
        //    {0,0,128},      // Blue
        //    {0,0,255},      // Bright Blue
        //    {128,0,0},      // Red
        //    {128,0,128},    // Magenta
        //    {128,0,255},    // Mauve
        //    {255,0,0},      // Bright Red
        //    {255,0,128},    // Purple
        //    {255,0,255},    // Bright Magenta
        //    {0,128,0},      // Green
        //    {0,128,128},    // Cyan
        //    {0,128,255},    // Sky Blue
        //    {128,128,0},    // Yellow
        //    {128,128,128},  // White
        //    {128,128,255},  // Pastel Blue
        //    {255,128,0},    // Orange
        //    {255,128,128},  // Pink
        //    {255,128,255},  // Pastel Magenta
        //    {0,255,0},      // Bright Green
        //    {0,255,128},    // Sea Green
        //    {0,255,255},    // Bright Cyan
        //    {128,255,0},    // Lime
        //    {128,255,128},  // Pastel Green
        //    {128,255,255},  // Pastel Cyan
        //    {255,255,0},    // Bright Yellow 
        //    {255,255,128},  // Pastel Yellow
        //    {255,255,255}   // Bright White
        //};

        // To change the foreground colour.
        public static void FgColour(int n)
        {
            if (n == 15) n = 12;
            if (n == 25) n = 14;
            if (n == 10) n = 9;

            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorList[n]);

            // Console.Write("\x1b[38;2;" + palette[n, 0] + ";" + palette[n, 1] + ";" + palette[n, 2] + "m");
        }

        // To change the background colour.
        public static void BgColour(int n)
        {

            if (n == 42) n = 12;
            if (n == 52) n = 14;
            if (n == 37) n = 9;
            if (n == 27) n = 0;

            Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorList[n]);

            // Console.Write("\x1b[48;2;" + palette[n - 27, 0] + ";" + palette[n - 27, 1] + ";" + palette[n - 27, 2] + "m");
        }

    }
}