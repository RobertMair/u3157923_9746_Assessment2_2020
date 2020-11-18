using ClassLibrary;
using System;
using System.ComponentModel;

namespace u3157923_9746_Assessment2
{
    public class PlayerControl
    {
        public static int playAreaX;
        public static int playAreaY;
        public static int PlayerInput(GuiInput.ConsoleHandle handle, ref GuiInput.INPUT_RECORD record, ref uint recordLen, ref bool focusButton)
        {
            if (!GuiInput.ReadConsoleInput(handle, ref record, 1, ref recordLen)) throw new Win32Exception();

            switch (record.EventType)
            {

                // To capture/display user keyboard input.
                case GuiInput.KEY_EVENT:
                    {
                        int key;

                        key = record.KeyEvent.bKeyDown ? record.KeyEvent.wVirtualKeyCode : 0;

                        // Player Up key pressed
                        if (key == 38 || key == 87)
                        {
                            return (int)PlayerEvent.MoveUp;
                        }

                        // Player Down key pressed
                        if (key == 40 || key == 83)
                        {
                            return (int)PlayerEvent.MoveDown;
                        }

                        // Player Left key pressed
                        if (key == 37 || key == 65)
                        {
                            return (int)PlayerEvent.MoveLeft;
                        }

                        // Player Right key pressed
                        if (key == 39 || key == 68)
                        {
                            return (int)PlayerEvent.MoveRight;
                        }

                        if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape) return 5;

                    }
                    break;

                case GuiInput.MOUSE_EVENT:
                    {
                        // Obtain number of any GuiElement the mouse cursor is above (if not above a GuiElement, it will return 0).
                        GameLoop.guiElement = GUI.GuiElementGrid[record.MouseEvent.dwMousePosition.Y, record.MouseEvent.dwMousePosition.X];

                        int returnValue = 0;

                        if (GameLoop.guiElement > 0)
                        {
                            // Unfocus a button when it has focus but mouse has now moved off but onto another button.
                            if (focusButton && MenuButtons.buttonNumber != GameLoop.guiElement - 1)
                            {
                                focusButton = false;
                                returnValue = (int)PlayerEvent.ButtonFocus;
                            }

                            // Focus a button when the mouse has moved over (unfocuses any previously focused button).
                            if (focusButton == false)
                            {
                                returnValue = (int)PlayerEvent.ButtonUnfocus; ;
                            }

                            focusButton = true;

                            // Trigger button event if button has focus and mouse is clicked.
                            if (record.MouseEvent.dwButtonState == 1)
                            {
                                returnValue = (int)PlayerEvent.ButtonClick;
                            }
                        }
                        else
                        {
                            // Unfocus all any focussed button when mouse is not above any button.
                            if (MenuButtons.buttonNumber != -1)
                            {
                                returnValue = (int)PlayerEvent.ButtonAllUnfocus;
                            }

                            focusButton = false;
                        }

                        // If the mouse is above the 'play area grid', which has the reserved GuiElement value of -1.
                        if (GameLoop.guiElement == -1)
                        {
                            playAreaX = record.MouseEvent.dwMousePosition.X - Grid.GridXOrg;
                            playAreaY = record.MouseEvent.dwMousePosition.Y - Grid.GridYOrg;
                            if (playAreaX < Grid.NodeGrid[0].Count && playAreaY < Grid.NodeGrid.Count)
                            {
                                if (record.MouseEvent.dwButtonState == 1
                                    && NodeContentCollection.NodeContents[Grid.NodeGrid[playAreaY][playAreaX]].Character == '.'
                                    && Grid.CurrentNodeChar == 'W' && Player.wall > 0
                                    )
                                {
                                    returnValue = (int)PlayerEvent.PlaceWall;
                                }

                                if (record.MouseEvent.dwButtonState == 1
                                    && NodeContentCollection.NodeContents[Grid.NodeGrid[playAreaY][playAreaX]].Character == 'W'
                                    && Grid.CurrentNodeChar == 'r' && Player.removeWall > 0
                                )
                                {
                                    returnValue = (int)PlayerEvent.RemoveWall;
                                }

                            }
                        }

                        return returnValue;
                    }
            }

            return (int)PlayerEvent.NoEvent;
        }
    }
}