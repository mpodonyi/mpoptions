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
    public class ValidationTest
    {
        public ValidationTest()
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
            Command root = Command.GetRoot();
            Assert.IsTrue(root.IsRoot);
        }


        [TestMethod]
        public void AddCommand_AddsNewCommand_ReturnsCurrentCommand()
        {
            Command cmd = Command.GetRoot().AddCommand("test", "testtoken");
            Assert.AreEqual("test", cmd.Name);
            Assert.AreEqual("testtoken", cmd.Token);
            Assert.IsFalse(cmd.IsRoot);
            Assert.IsTrue(cmd.ParentCommand.IsRoot);
            Assert.IsTrue(cmd.RootCommand.IsRoot);
        }

        [TestMethod]
        public void CommandsIndexer_GetChildCommandByName_ReturnsSingleChildCommand()
        {
            Command cmd = Command.GetRoot().AddCommand("test", "testtoken");
            Assert.AreSame(cmd, cmd.ParentCommand.Commands["test"]);
        }

        [TestMethod]
        public void CommandsEnumerator_Execute_ReturnsAllChildCommand()
        {
            Command root = Command.GetRoot();
            Command cmd1 = root.AddCommand("test1", "testtoken1");
            Command cmd2 = root.AddCommand("test2", "testtoken2");

            CollectionAssert.Contains(root.Commands, cmd1);
            CollectionAssert.Contains(root.Commands, cmd2);
            Assert.AreEqual(2, root.Commands.Count);
        }





        [TestMethod]
        public void AddCommand_InvalidToken_ThrowsException()
        {
            string[] validTokens = new[] {"test;token",
                 "test ;token", 
                 "test; token",
                 "test; ",
                 "test;",
                 "test ",
                 " test",
                 " test ",
             };

            foreach (string validToken in validTokens)
            {
                Command cmd = Command.GetRoot().AddCommand("test", validToken);
                Assert.AreEqual(validToken, cmd.Token);
            }

            string[] invalidTokens = new[] {"-test;token",
                 "test ;--token", 
                 "test; /token",
                 "/test; ",
                 "-test;",
                 "/test ",
                 " -test",
                 " -test ",
             };

            foreach (string invalidToken in invalidTokens)
            {
                AssertHelper.Throws<ArgumentException>(() =>
                {
                    Command cmd = Command.GetRoot().AddCommand("test", invalidToken);
                });


            }
        }

        [TestMethod]
        public void AddCommand_DuplicateToken_ThrowsException()
        {
            Command cmd = Command.GetRoot();

            cmd.AddCommand("test", "mike;was;here");
            AssertHelper.Throws<ArgumentException>(() => { cmd.AddCommand("test2", "john;was;not"); });
        }

        [TestMethod]
        public void AddCommand_DuplicateName_ThrowsException()
        {
            Command cmd = Command.GetRoot();

            cmd.AddCommand("test", "mike;was;here");
            AssertHelper.Throws<ArgumentException>(() => { cmd.AddCommand("test", "john;wasnt;not"); });
        }

        [TestMethod]
        public void AddCommand_TestHierarchy_ReturnsCommandsInHierarchy()
        {
            Command cmd = Command.GetRoot().AddCommand("test1", "mike;was;here").AddCommand("test", "mike;was;here");

            Assert.AreEqual("test", cmd.Name);
            Assert.AreEqual("test1", cmd.ParentCommand.Name);
            Assert.IsTrue(cmd.ParentCommand.ParentCommand.IsRoot);
        }

        [TestMethod]
        public void AddGlobalOption_Execute_ReturnsGlobalOption()
        {
            Command cmd = Command.GetRoot().AddGlobalOption("globop", "s;k").RootCommand;
            Assert.IsTrue(cmd.Options["globop"].IsGlobalOption);

        }

        [TestMethod]
        public void AddGlobalOption_AddedGlobalOptionIsGivenBackInWholeHierarchy_ReturnCommand()
        {
            Command cmd = Command.GetRoot().AddGlobalOption("globop", "s;k").AddCommand("mike", "fire").AddOption("locop", "v;l").ParentCommand;
            Assert.IsFalse(cmd.IsRoot);
            Assert.IsNotNull(cmd.Options["locop"]);
            Assert.IsTrue(cmd.Options["globop"].IsGlobalOption);
            Assert.AreEqual(2, cmd.Options.Count);
        }

        [TestMethod]
        public void AddOption_Execute_ReturnsOption()
        {
            Command cmd = Command.GetRoot().AddOption("globop", "s;k").ParentCommand;
            Assert.IsNotNull(cmd.Options["globop"]);

        }

        [TestMethod]
        public void AddOption_NameAlreadyNameOfGlobalOption_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddGlobalOption("mike", "t1");
            AssertHelper.Throws<ArgumentException>(() => cmd.AddOption("mike", "t2"));

            cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1");
            AssertHelper.Throws<ArgumentException>(() => cmd.AddGlobalOption("mike", "t2"));
        }


        [TestMethod]
        public void AddOption_TokenAlreadyTokenOfGlobalOption_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddGlobalOption("mike", "t1").AddOption("mike1", "t1");

            ParserErrorContext error=null;
            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1",out error));
            Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_OptionWithRegularExpressionAlreadyExist_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1").WithRegexValidator(@"\d").AddOption("mike1", "t1").WithRegexValidator(@"\d");

            ParserErrorContext error = null;
            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1", out error));
            Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_OptionWhereStaticValueOfStaticValueValidatorExistAlready_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1").WithStaticValidator("mike", "was").AddOption("mike1", "t1").WithStaticValidator("john", "was");

            ParserErrorContext error = null;
            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1:was",out error));
            Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_OptionWhereStaticValueOfStaticValueValidatorDoesNotExistForOptionWithSameToken_Successful()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1").WithStaticValidator("mike", "was").AddOption("mike1", "t1").WithStaticValidator("john", "not");

            ParserErrorContext error = null;
            AssertHelper.ThrowsNoException(() => cmd.Parse(" -t1:was", out error));
            Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_2OptionsWithSameValueBut1WithValidatorand1Without_ThrowsException()
        {
            Command cmd;
            ParserErrorContext error = null;

            cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1").AddOption("mike1", "t1").WithStaticValidator("john", "was");
            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1", out error));
            Assert.IsNull(error);

            cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1").AddOption("mike2", "t1").WithRegexValidator("\\d");
            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1", out error));
            Assert.IsNull(error);
       }

        [TestMethod]
        public void AddOption_2OptionsWithSameValueAndNoValidator_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1;t2;t3").AddOption("mike1", "t4;t2;t5");

            ParserErrorContext error = null;
            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1", out error));
            Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_2OptionsWithTokenAndValueOptional_ThrowsException()
        {
            ParserErrorContext error = null;
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1;t2;t3").WithStaticValidator(true, "x", "y").AddOption("mike2", "t4;t2;t5").WithRegexValidator(@"^\d+$", true);

            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1:x",out error));
            Assert.IsNull(error);

            cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1;t2;t3").AddOption("mike2", "t4;t2;t5").WithRegexValidator(@"^\d+$", true);

            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1",out error));
            Assert.IsNull(error);

            //cmd = Command.GetRoot();
            //cmd.AddOption("mike", "t1;t2;t3").WithStaticValidator("x", "y").AddOption("mike2", "t4;t2;t5").WithRegexValidator(@"^\d+$", true);

            //AssertHelper.ThrowsNoException(() => cmd.Parse(" -t1:x",out error));
            //Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_InvalidToken_ThrowsException()
        {
            AssertHelper.Throws<ArgumentException>(() => Command.GetRoot().AddOption("mike", "-test"));
            AssertHelper.Throws<ArgumentException>(() => Command.GetRoot().AddOption("mike", "te st"));
            AssertHelper.Throws<ArgumentException>(() => Command.GetRoot().AddOption("mike", "test="));
            AssertHelper.Throws<ArgumentException>(() => Command.GetRoot().AddOption("mike", "test:"));

            AssertHelper.ThrowsNoException(() => Command.GetRoot().AddOption("mike", "?"));
            AssertHelper.ThrowsNoException(() => Command.GetRoot().AddOption("mike", " ? "));
            AssertHelper.ThrowsNoException(() => Command.GetRoot().AddOption("mike", " te?st"));
            AssertHelper.ThrowsNoException(() => Command.GetRoot().AddOption("mike", "/"));
        }

        [TestMethod]
        public void AddGlobalOption_GlobalOptionWithNameOfLocalOption_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd = cmd.AddCommand("mikecom", "test").AddOption("mikeopt", "t1;t2;t3").RootCommand;

            AssertHelper.Throws<ArgumentException>(() => cmd.AddGlobalOption("mikeopt", "t4"));
        }

        [TestMethod]
        public void AddGlobalOption_GlobalOptionWithTokenOfLocalOption_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddCommand("mikecom", "test").AddOption("mikeopt", "t1;t2;t3").RootCommand.AddGlobalOption("mikeglobopt", "t4;t2;t3");

            ParserErrorContext error = null;
            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1",out error));
            Assert.IsNull(error);
        }

        [TestMethod]
        public void AddArgument_Execute_ReturnsArgument()
        {
            Command cmd = Command.GetRoot().AddArgument("arg").WithRegexValidator(@"^\d+$").ParentCommand;
            Assert.IsNotNull(cmd.Arguments["arg"]);
        }

        [TestMethod]
        public void AddArgument_MoreThenOneArgumentForCommand_ThrowsException()
        {
            var cmd = Command.GetRoot().AddArgument("arg");
            AssertHelper.Throws<ArgumentException>(()=>cmd.AddArgument("arg2"));
        }

        [TestMethod]
        public void AddOption_AfterGlobalOptionInFluentStyleAddsOptionToParentCommand_Successful()
        {
            Command rootcommand = Command.GetRoot();

            //Adds Option to command test
            AssertHelper.ThrowsNoException(() =>
            {
                rootcommand.AddCommand("test", "mike").AddGlobalOption("globopt", "gl").AddOption("option", "opt");
            });

            Assert.IsNotNull(rootcommand.Commands["test"].Options["option"]);

            Assert.AreEqual(rootcommand.Commands["test"], rootcommand.Commands["test"].Options["option"].ParentCommand);

        }

        [TestMethod]
        public void AddOption_AfterGlobalOptionGetByIndexerAddsOptionToContextParentCommand_Successful()
        {
            Command rootcommand = Command.GetRoot();

            rootcommand.AddCommand("test", "mike").AddGlobalOption("globopt", "gl");

            AssertHelper.ThrowsNoException(() =>
            {
                rootcommand.Commands["test"].Options["globopt"].AddOption("opt2", "opt2");
            });

            Assert.IsNotNull(rootcommand.Commands["test"].Options["opt2"]);

            Assert.AreEqual(rootcommand.Commands["test"], rootcommand.Commands["test"].Options["opt2"].ParentCommand);

            Option option = rootcommand.Commands["test"].Options["globopt"];

            Assert.IsTrue(option.IsGlobalOption);

            Assert.AreEqual(rootcommand, option.ParentCommand);
        }

        [TestMethod]
        public void AddOption_OverSavedInstanceOfGlobalOptionAddsToContextCommand_Successful()
        {

            Command rootcommand = Command.GetRoot();

            rootcommand.AddCommand("com1", "com").AddCommand("com2", "com").AddGlobalOption("globopt", "gopt");

            Option option1 = rootcommand.Commands["com1"].Options["globopt"];

            Option option2 = rootcommand.Commands["com1"].Commands["com2"].Options["globopt"];

            option1.AddOption("option", "opt");

            option2.AddOption("option2", "opt");

            Assert.IsNotNull(rootcommand.Commands["com1"].Options["option"]);

            Assert.IsNotNull(rootcommand.Commands["com1"].Commands["com2"].Options["option2"]);
        }

        [TestMethod]
        public void AddOption_OptionWithValidatorHasAlreadyOtherOptionsWithValidatorWithSameTokenValue_ThrowsException()
        {
            ParserErrorContext error = null;
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t2").WithRegexValidator(@"\d").AddOption("mike1", "t1").WithNoValidator();
            AssertHelper.ThrowsNoException(() => cmd.Parse(" -t1:y7",out error));
            Assert.IsNull(error);

            cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1").WithRegexValidator(@"\d").AddOption("mike1", "t1").WithNoValidator();
            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1:y7", out error));
            Assert.IsNull(error);
        }

        [TestMethod]
        public void Parse_2OptionsWithSameValueBut1WithValidatorand1Without_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1").AddOption("mike1", "t1").WithStaticValidator("john", "was");

            ParserErrorContext error;
            AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1", out error));
        }
    }
}
