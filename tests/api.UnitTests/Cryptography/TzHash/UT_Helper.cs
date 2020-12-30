using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoFS.API.v2.Cryptography.Tz;

namespace TzHash.UnitTests
{
    [TestClass]
    public class UT_Helper
    {
        [TestMethod]
        public void TestGetLeadingZeros()
        {
            ulong u1 = 1;
            int i = Helper.GetLeadingZeros(u1);
            Assert.AreEqual(63, i);
        }
    }
}
