using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary
{
    public static class Grid
    {

        public static List<char> Nodes;
        public static char DefaultNodeChar; // Default character to place in 'grid' at each 'node' when first creating the grid.
        public static char CurrentNodeChar; // Currently selected character to place in 'grid' at each 'node'.
        public static int GridXOrg; // Set X origin of 'grid' (area used for maps/pathfinding).
        public static int GridYOrg; // Set Y origin of 'grid' (area used for maps/pathfinding).
        public static int GridXSize; // Set maximum width size of 'grid'.
        public static int GridYSize; // Set maximum height size of 'grid'.

        public static List<List<int>> NodeGrid = new List<List<int>>(); // List of lists, to hold 'node' content for the grid.

        public static void GridStartNodes(List<char> newnodes)
        {
            Nodes = newnodes.ToList();
        }

        public static void CreateGrid()
        {
            int nodeNumber = 0;

            for (var i = 0; i < GridYSize; i++)
            {
                NodeGrid.Add(new List<int>());
                for (var j = 0; j < GridXSize; j++) NodeGrid[i].Add(NodeContentCollection.CreateNodeInstance(Nodes[nodeNumber++]));
            }
            RenderGrid(GridXOrg, GridYOrg);
        }

        public static void RenderGrid(int xOrg, int yOrg)
        {
            // Example of 'hoisting' for iterating over a 2d list.
            var height = NodeGrid.Count;
            var width = NodeGrid[0].Count;
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    RenderNode(xOrg, yOrg, x, y);
                    GUI.GuiElementGrid[yOrg + y, xOrg + x] = -1;
                }
        }

        // Render grid node content at node location in grid.
        public static void RenderNode(int xOrg, int yOrg, int x, int y)
        {
            Console.SetCursorPosition(xOrg + x, yOrg + y);
            ColourPalette.FgColour(NodeContentCollection.NodeContents[NodeGrid[y][x]].FgColour);
            ColourPalette.BgColour(NodeContentCollection.NodeContents[NodeGrid[y][x]].BgColour);
            Console.Write((char)NodeContentCollection.NodeContents[NodeGrid[y][x]].Character);
        }

        // Grid Class generic dependency Injection methods for GuiElement class objects.
        public class ResetGrid : IAction
        {
            public void Action(params object[] list)
            {
                NodeGrid.Clear();
                NodeContentCollection.NodeContents.Clear();
                CreateGrid();
            }
        }

    }
}