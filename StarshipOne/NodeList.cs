using ClassLibrary;

namespace u3157923_9746_Assessment2
{
    public class NodeList
    {
        // Create a list of type 'NodeContentTypes' (use as 'prototypes' for creating 'NodeContentTypes' class instances to place into grid.
        public static NodeContentTypeCollection NodeContentTypes = new NodeContentTypeCollection();

        public static void SetNodes()
        {

            // Collectible node types.
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('z', "ZnCl2 cell", true, -1, -1, 25, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('e', "Energy cell", true, -1, 49, 25, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('o', "Oxygen cell", true, 199, -1, 25, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('k', "Key", true, -1, -1, 25, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('w', "Temporary wall", true, -1, -1, 25, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('r', "Remove wall", true, -1, -1, 25, 37, -1, -1, 1, -1, 1);


            // Terrain node types.
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('-', "space", false, 0, 0, 0, 27, -1, 5, 5, 5, 5);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('.', "floor", true, -1, -1, 10, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('#', "wall", false, 0, 0, 15, 42, -1, 5, 5, 5, 5);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('W', "Temporary wall", false, 0, 0, 0, 42, -1, 5, 5, 5, 5);

            NodeContentTypes[NodeContentTypes.Count] = new GridNode('+', "door", true, -1, -5, 15, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('*', "locked door", false, -5, -15, 15, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('?', "info terminal", true, -5, -15, 0, 42, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode(',', "acid", true, -5, -50, 15, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('~', "ice", true, -1, -1, 15, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('_', "force field", true, -15, -50, 15, 37, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('|', "force field", true, -15, -50, 15, 37, -1, -1, 1, -1, 1);

            NodeContentTypes[NodeContentTypes.Count] = new GridNode('^', "door up", true, -1, -5, 15, 37, -1, -1, 5, 5, 5);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('v', "door down", true, -1, -5, 15, 37, -1, 5, 1, 5, 5);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('<', "door left", true, -1, -5, 15, 37, -1, 5, 5, -1, 5);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('>', "door right", true, -1, -5, 15, 37, -1, 5, 5, 5, 1);

            NodeContentTypes[NodeContentTypes.Count] = new GridNode('A', "airlock", true, -5, -5, 0, 42, -1, -1, 1, -1, 1);
            NodeContentTypes[NodeContentTypes.Count] = new GridNode('L', "lift", true, -5, -5, 0, 42, -1, -1, 1, -1, 1);

        }
    }
}