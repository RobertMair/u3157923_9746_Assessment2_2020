using Microsoft.VisualStudio.TestTools.UnitTesting;
using u3157923_9746_Assessment2;

namespace UnitTests
{
    [TestClass]
    public class NodeListTests
    {
        [TestMethod]
        public void Test_NodeList()
        {
            const int expectedElements = 23;
            NodeList.SetNodes();
            Assert.IsNotNull(NodeList.NodeContentTypes);
            Assert.AreEqual(expectedElements, NodeList.NodeContentTypes.Count, $"Expected {expectedElements} elements");
        }
    }
}