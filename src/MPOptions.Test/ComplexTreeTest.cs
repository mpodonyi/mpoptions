using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPOptions.Test
{
    /// <summary>
    /// Summary description for ValidationTest
    /// </summary>
    [TestClass]
    public class ComplexTreeTest
    {
        public ComplexTreeTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private RootCommand root = MPOptions.GetRoot()
            .Add(
                new Command("com1", "tok1")
                    .Add(
                        new Option("opt1", "o1", false),
                        new Option("opt2", "o2", false),
                        new Option("opt3", "o3", false).WithRegexValidator("/d")
                    ),
                new Command("com2", "tok2")
                    .Add(
                        new Command("com1", "tok1"))
                    .Add(
                        new Argument("arg1")
                    ),
                new Command("com3", "tok3")
            );


        //private RootCommand root2 = MPOptions.GetRoot().Add(
        //    new Command("com1", "tok1").Add(
        //        new Option("opt1", "-o1", false),
        //        new Option("opt2", "-o2", false),
        //        new Option("opt3", "-o3", false).WithRegexValidator("/d")
        //        ))
        //    .Add(
        //        new Option("EEE", "fgfg", false));


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetRoot_Execute_ReturnsRootCommand()
        {
            root.Options.Clear();
           
        }


     
    }
}
