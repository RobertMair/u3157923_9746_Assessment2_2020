using ClassLibrary;
using System;
using System.Collections.Generic;

namespace u3157923_9746_Assessment2
{
    public class MenuButtons
    {
        public static readonly List<GuiElement> ButtonList = new List<GuiElement>();
        public static int buttonNumber = -1; // Used to hold a value of any GUI element 'button' that has come into 'focus' so it can be 'unfocused'.

        public static void CreateMenuButtons()
        {

            // Game Menu Buttons: Player inventory/actions.

            ButtonList.Add(new GuiElement(69, 30, 8, 1, " WALL", 25, 42, ButtonList.Count + 1, true, new PlaceWall()));
            ButtonList.Add(new GuiElement(69, 32, 8, 1, " REMOVE", 25, 42, ButtonList.Count + 1, true, new RemoveWall()));

            // Game Menu Buttons: In game options.

            ButtonList.Add(new GuiElement(68, 36, 10, 2, " MUSIC` ON", 25, 42, ButtonList.Count + 1, true, new MusicStart()));
            ButtonList.Add(
                new GuiElement(79, 36, 10, 2, " SOUND FX` ON", 25, 42, ButtonList.Count + 1, true, new SoundFXOn()));
            ButtonList.Add(
                new GuiElement(90, 36, 10, 2, " RESUME` GAME", 25, 42, ButtonList.Count + 1, true, new ResumeGame()));

            ButtonList.Add(new GuiElement(68, 39, 10, 2, " MUSIC` OFF", 25, 42, ButtonList.Count + 1, true, new MusicStop()));
            ButtonList.Add(new GuiElement(79, 39, 10, 2, " SOUND FX` OFF", 25, 42, ButtonList.Count + 1, true,
                new SoundFXOff()));
            ButtonList.Add(new GuiElement(90, 39, 10, 2, " PAUSE` GAME", 25, 42, ButtonList.Count + 1, true,
                new PauseGame()));

            ButtonList.Add(new GuiElement(89, 10, 8, 1, "  QUIT", 25, 42, ButtonList.Count + 1, true, new ExitProgram()));
        }

        // This project dependency Injection methods for GuiElement class objects.


        public class MusicStart : IAction
        {
            public void Action(params object[] list)
            {
                // Sound.SoundPlayer.PlayLooping();
                Sound.music.PlaySound();
            }
        }
        public class MusicStop : IAction
        {
            public void Action(params object[] list)
            {
                // Sound.SoundPlayer.Stop();
                Sound.music.Stop();
            }
        }

        public class SoundFXOn : IAction
        {
            public void Action(params object[] list)
            {
                Sound.soundFxOn = true;
            }
        }

        public class SoundFXOff : IAction
        {
            public void Action(params object[] list)
            {
                Sound.soundFxOn = false;
            }
        }

        public class PlaceWall : IAction
        {
            public void Action(params object[] list)
            {
                Grid.CurrentNodeChar = 'W';
                GUI.TextWrite(68, 30, ">", 25, 27, 0);
                GUI.TextWrite(68, 32, " ", 25, 27, 0);
            }
        }

        public class RemoveWall : IAction
        {
            public void Action(params object[] list)
            {
                Grid.CurrentNodeChar = 'r';
                GUI.TextWrite(68, 30, " ", 25, 27, 0);
                GUI.TextWrite(68, 32, ">", 25, 27, 0);
            }
        }

        public class PauseGame : IAction
        {
            public void Action(params object[] list)
            {

                GUI.pauseGame = true;
            }
        }
        public class ResumeGame : IAction
        {
            public void Action(params object[] list)
            {

                GUI.pauseGame = false;
            }
        }
        public class ExitProgram : IAction
        {
            public void Action(params object[] list)
            {
                GameLoop.endProgram = true;
                Console.Clear();
                Console.Write("\n Thank you for using this program, press any key to fully exit...");
                Console.ReadKey();
            }
        }

    }
}