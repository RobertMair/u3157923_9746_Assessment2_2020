using ClassLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using u3157923_9746_Assessment2;

namespace UnitTests
{
    [TestClass]
    public class PlayerControlTests
    {
        [TestMethod]
        public void Test_PlayerControl_Instantiation()
        {
            var inputRecord = new GuiInput.INPUT_RECORD { EventType = GuiInput.KEY_EVENT };
            uint recordLen = 1;
            var f = true;
            var result = PlayerControl.PlayerInput(new GuiInput.ConsoleHandle(), ref inputRecord, ref recordLen, ref f);
            Assert.IsNotNull(result, "expecting result");
            Assert.AreEqual(0, result, "expecting 0");
        }
    }
}
