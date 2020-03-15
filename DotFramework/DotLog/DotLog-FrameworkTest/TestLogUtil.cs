// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using Dot.Log;
using NUnit.Framework;
using System.IO;

namespace DotLog_FrameworkTest
{
    [TestFixture]
    public class TestLogUtil
    {
        private static readonly string CONFIG_PATH = "./DotLog-FrameworkTest/log_config.xml";

        [OneTimeSetUp]
        public void TestSetUp()
        {
            LogUtil.Initalize(File.ReadAllText(CONFIG_PATH), LogLevelType.Info);
        }



        [Test]
        public void TestLogError()
        {
            LogUtil.LogError("TestTag", "Just for Test");
            LogUtil.LogError("TestTag", "Just for Test");
            LogUtil.LogError("TestTag", "Just for Test");
        }
    }
}
