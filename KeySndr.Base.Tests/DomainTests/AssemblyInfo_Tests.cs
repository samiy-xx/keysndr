using KeySndr.Base.Domain;
using NUnit.Framework;

namespace KeySndr.Base.Tests.DomainTests
{
    [TestFixture]
    public class AssemblyInfo_Tests
    {
        [Test]
        public void ReturnsCorrect()
        {
            var t = AssemblyInfo.GetAssemblyVersion(typeof (KeySndrApp));
            Assert.IsNotNull(t);
        }
    }
}
