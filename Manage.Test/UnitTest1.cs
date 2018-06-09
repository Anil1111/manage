using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Manage.Core.Utility;
using System.Web;

namespace Manage.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string path = StringUtil.PinYin("贾海洋");
        }
    }
}
