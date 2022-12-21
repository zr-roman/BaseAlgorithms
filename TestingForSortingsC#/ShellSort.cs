﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortings;

namespace Testing {

    [TestClass]
    public class ShellSort : TestingBase {

         [TestMethod]
        public void TestAsc()
        {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.ShellSort(arr, Order.ASC);
                AssertionAsc();
            }
        }

        [TestMethod]
        public void TestDesc()
        {
            while (N-- > 0)
            {
                GenerateNewArray();
                Lib.ShellSort(arr, Order.DESC);
                AssertionDesc();
            }
        }

    }
}