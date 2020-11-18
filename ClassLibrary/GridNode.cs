using System.Collections.Generic;

namespace ClassLibrary
{

    public interface INodeContent
    {
        INodeContent Clone();
    }

    // For pathfinding, each grid node can have a 'node type' to place into it, these use this class design.
    public class GridNode : INodeContent
    {
        public char Character { get; set; }
        public string Description { get; set; }
        public bool Walkable { get; set; }
        public int Oxygen { get; set; }
        public int Energy { get; set; }
        public int FgColour { get; set; }
        public int BgColour { get; set; }
        public int Allowed { get; set; } // Total allowed on grid. A value of -1 indicates any amount.
        public int Up { get; set; } // These four parameters determine which way a walkable node can be passed through.
        public int Down { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }


        // Constructor: for prototype's for creation of instances to be placed into the grid.
        public GridNode(char character, string description, bool walkable, int oxygen, int energy, int fgcolour, int bgcolour, int allowed, int up, int down, int left, int right)
        {
            Character = character;
            Description = description;
            Walkable = walkable;
            Oxygen = oxygen;
            Energy = energy;
            FgColour = fgcolour;
            BgColour = bgcolour;
            Allowed = allowed;
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }

        // Constructor: for instances placed into the grid.
        public INodeContent Clone()
        {
            return (INodeContent)MemberwiseClone();
        }
    }

    // Create 'node prototype' list that can be cloned and used in the grid.
    public class NodeContentTypeCollection
    {
        private static readonly List<GridNode> NodeContentTypes = new List<GridNode>();

        public GridNode this[int index]
        {
            get => NodeContentTypes[index];
            set => NodeContentTypes.Insert(index, value);
        }

        public int Count => NodeContentTypes.Count;

        public static GridNode Clone(char nodeChar)
        {
            var node = NodeContentTypes.FindIndex(x => x.Character.Equals(nodeChar));
            return (GridNode)NodeContentTypes[node].Clone();
        }

        public static int FindIndex(char nodeChar)
        {
            return NodeContentTypes.FindIndex(x => x.Character.Equals(nodeChar));
        }

        public static bool CheckNodeAllowed(char nodeChar)
        {
            var node = FindIndex(nodeChar);

            if (NodeContentTypes[node].Allowed == -1) return true;
            var count = 0;
            foreach (var t in Grid.NodeGrid)
                foreach (var t1 in t)
                    if (NodeContentCollection.NodeContents[t1].Character == nodeChar)
                        count++;

            return count < NodeContentTypes[node].Allowed;
        }

    }

    // Collection of node content instances used in the grid.
    public static class NodeContentCollection
    {
        public static List<GridNode> NodeContents = new List<GridNode>();
        public static int Count => NodeContents.Count;

        public static void Clear()
        {
            NodeContents.Clear();
        }

        public static int CreateNodeInstance(char nodeChar)
        {
            NodeContents.Add(NodeContentTypeCollection.Clone(nodeChar));
            return NodeContents.Count - 1;
        }

    }
}