using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPOptions.Test
{
    /// <summary>
    /// Summary description for ParserTest
    /// </summary>
    [TestClass]
    public class ParserTest
    {
        public ParserTest()
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
                AssertHelper.Throws<ArgumentOutOfRangeException>(() =>
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
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => { cmd.AddCommand("test2", "john;was;not"); });
        }

        [TestMethod]
        public void AddCommand_DuplicateName_ThrowsException()
        {
            Command cmd = Command.GetRoot();

            cmd.AddCommand("test", "mike;was;here");
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => { cmd.AddCommand("test", "john;wasnt;not"); });
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
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike", "t2"));

            cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1");
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddGlobalOption("mike", "t2"));
        }

        //[TestMethod]
        //public void AddOption_TokenAlreadyTokenOfGlobalOption_ThrowsException()
        //{
        //    Command cmd = Command.GetRoot();
        //    cmd.AddGlobalOption("mike", "t1");
        //    Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike1", "t1"));
        //}

        [TestMethod]
        public void AddOption_OptionWithRegularExpressionAlreadyExist_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1", new RegularExpressionOptionValueValidator(@"\d"));
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike1", "t1", new RegularExpressionOptionValueValidator(@"\d")));
        }

        [TestMethod]
        public void AddOption_OptionWhereStaticValueOfStaticValueValidatorExistAlready_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1", new StaticOptionValueValidator("mike", "was"));
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike1", "t1", new StaticOptionValueValidator("john", "was")));
        }

        [TestMethod]
        public void AddOption_OptionWhereStaticValueOfStaticValueValidatorDoesNotExistForOptionWithSameToken_Successful()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1", new StaticOptionValueValidator("mike", "was"));
            AssertHelper.ThrowsNoException(() => cmd.AddOption("mike1", "t1", new StaticOptionValueValidator("john", "not")));
        }

        [TestMethod]
        public void AddOption_2OptionsWithSameValueBut1WithValidatorand1Without_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1");
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike1", "t1", new StaticOptionValueValidator("john", "was")));

            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike2", "t1", new RegularExpressionOptionValueValidator("\\d")));

            cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1", new StaticOptionValueValidator("john", "was"));
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike1", "t1"));
        }

        [TestMethod]
        public void AddOption_2OptionsWithSameValueAndNoValidator_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1;t2;t3");
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike1", "t4;t2;t5"));
        }

        [TestMethod]
        public void AddOption_2OptionsWithTokenAndValueOptional_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1;t2;t3", new StaticOptionValueValidator("x,y") { ValueOptional = true });

            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike2", "t4;t2;t5", new RegularExpressionOptionValueValidator(@"^\d+$") { ValueOptional = true }));

            cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1;t2;t3");

            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddOption("mike2", "t4;t2;t5", new RegularExpressionOptionValueValidator(@"^\d+$") { ValueOptional = true }));

            cmd = Command.GetRoot();
            cmd.AddOption("mike", "t1;t2;t3", new StaticOptionValueValidator("x,y"));

            AssertHelper.ThrowsNoException(() => cmd.AddOption("mike2", "t4;t2;t5", new RegularExpressionOptionValueValidator(@"^\d+$") { ValueOptional = true }));
        }

        [TestMethod]
        public void AddOption_InvalidToken_ThrowsException()
        {
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => Command.GetRoot().AddOption("mike", "-test"));
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => Command.GetRoot().AddOption("mike", "te st"));
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => Command.GetRoot().AddOption("mike", "test="));
            AssertHelper.Throws<ArgumentOutOfRangeException>(() => Command.GetRoot().AddOption("mike", "test:"));

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

            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddGlobalOption("mikeopt", "t4"));
        }

        [TestMethod]
        public void AddGlobalOption_GlobalOptionWithTokenOfLocalOption_ThrowsException()
        {
            Command cmd = Command.GetRoot();
            cmd = cmd.AddCommand("mikecom", "test").AddOption("mikeopt", "t1;t2;t3").RootCommand;

            AssertHelper.Throws<ArgumentOutOfRangeException>(() => cmd.AddGlobalOption("mikeglobopt", "t4;t2;t3"));
        }

        [TestMethod]
        public void AddArgument_Execute_ReturnsArgument()
        {
            Command cmd = Command.GetRoot().AddArgument("arg", new RegularExpressionArgumentValidator(@"^\d+$")).ParentCommand;
            Assert.IsNotNull(cmd.Arguments["arg"]);
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
        public void AddOption_AddOptionOverSavedInstanceOfGlobalOptionAddsToContextCommand_Successful()
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

        //==============  Parser Tests

        private const string cmdline = "test.exe";

        [TestMethod]
        public void Parse_Command_Successful()
        {
            Command cmd = Command.GetRoot();
            cmd.AddCommand("testcommand", "testcommanda; testcommandb");

            bool error;
            cmd.Parse(" testcommandb", out error);

            Assert.IsFalse(error);
            Assert.IsTrue(cmd.Commands["testcommand"].IsSet);

        }

        [TestMethod]
        public void Parse_Command_Unsuccessful()
        {
            Command cmd = Command.GetRoot();
            cmd.AddCommand("testcommand", "testcommanda; testcommandb");

            bool error;
            cmd.Parse(" testcommandc", out error);

            Assert.IsTrue(error);
            Assert.IsFalse(cmd.Commands["testcommand"].IsSet);

        }

        [TestMethod]
        public void Parse_Option_Successful()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("testoption", "testoption; testoptionb");
            cmd.AddOption("testoption2", "testoptionc; testoptiond");
            cmd.AddOption("testoption3", "testoptione; testoptionf");

            bool error;
            cmd.Parse(@" --testoptionb /testoptionc -testoptione", out error);

            Assert.IsFalse(error);
            Assert.IsTrue(cmd.Options["testoption"].IsSet);
            Assert.IsTrue(cmd.Options["testoption2"].IsSet);
            Assert.IsTrue(cmd.Options["testoption3"].IsSet);

        }

        [TestMethod]
        public void Parse_Option_Unsuccessful()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("testoption", "testoption; testoptionb");
            cmd.AddOption("testoption2", "testoptionc; testoptiond");
            cmd.AddOption("testoption3", "testoptione; testoptionf");

            bool error;
            cmd.Parse(@" --testoptionb /testoptionx -testoptione", out error);

            Assert.IsTrue(error);
        }

        [TestMethod]
        public void Parse_OptionWithStaticValidator_Successful()
        {
            Command cmd = Command.GetRoot();
            cmd.AddOption("testoption", "testoption; testoptionb", new StaticOptionValueValidator("yes", "no"));
            var option = cmd.Options["testoption"];

            bool error;
            //cmd.Parse(@" --testoptionb:yes \testoptionc -testoptione", out error);
            cmd.Parse(@" --testoptionb:yes", out error);

            Assert.IsFalse(error);

            Assert.IsTrue(option.IsSet);
            Assert.AreEqual("yes", option.Value);

        }

        [TestMethod]
        public void Parse_OptionWithStaticValidatorAndAmper_Successful()
        {
            string[,] testvalues = {
                                       {"ye s"," --testoptionb:\"ye s\""},
                                       {"n\"oo"," --testoptionb:\"n\\\"oo\""},
                                       {@"Mike \ Was"," --testoptionb:\"Mike \\\\ Was\""},
                                       {"Mike \\\" Was"," --testoptionb:\"Mike \\\\\\\" Was\""},
                                       {@"Mike1 \ Was"," --testoptionb:\"Mike1 \\ Was\""},
                                   };

            for (int i = 0; i < testvalues.GetLength(0); i++)
            {
                Command cmd = Command.GetRoot();

                cmd.AddOption("testoption", "testoption; testoptionb", new StaticOptionValueValidator(testvalues[i, 0]));
                var option = cmd.Options["testoption"];

                bool error;

                cmd.Parse(testvalues[i, 1], out error);
                Assert.IsFalse(error);
                Assert.IsTrue(option.IsSet);
                Assert.AreEqual(testvalues[i, 0], option.Value);
            }
        }

        [TestMethod]
        public void Parse_OptionWithRegexValidator_Successful()
        {
            string[,] testvalues = {
                                       {"12345"," --testoptionb:12345"},
                                   };

            for (int i = 0; i < testvalues.GetLength(0); i++)
            {
                Command cmd = Command.GetRoot();

                cmd.AddOption("testoption", "testoption; testoptionb", new RegularExpressionOptionValueValidator(@"^\d+$"));
                var option = cmd.Options["testoption"];

                bool error;

                cmd.Parse(testvalues[i, 1], out error);
                Assert.IsFalse(error);
                Assert.IsTrue(option.IsSet);
                Assert.AreEqual(testvalues[i, 0], option.Value);
            }
        }

        [TestMethod]
        public void Parse_OptionWithRegexValidator_UnSuccessful()
        {
            string[,] testvalues = {
                                       {"12a45"," --testoptionb:12a45"},
                                   };

            for (int i = 0; i < testvalues.GetLength(0); i++)
            {
                Command cmd = Command.GetRoot();

                cmd.AddOption("testoption", "testoption; testoptionb", new RegularExpressionOptionValueValidator(@"^\d+$"));
                var option = cmd.Options["testoption"];

                bool error;

                cmd.Parse(testvalues[i, 1], out error);
                Assert.IsTrue(error);
                Assert.IsFalse(option.IsSet);
                Assert.AreNotEqual(testvalues[i, 0], option.Value);
            }
        }

        [TestMethod]
        public void Parse_OptionWithRegexValidatorAndMultipleValues_Successful()
        {
            string testvalues = " --testoptionb:12345 --testoptionb:7890";

            Command cmd = Command.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb", new RegularExpressionOptionValueValidator(@"^\d+$") { MaximumOccurrence = 2 });
            var option = cmd.Options["testoption"];

            bool error;

            cmd.Parse(testvalues, out error);
            Assert.IsFalse(error);
            Assert.IsTrue(option.IsSet);

            Assert.AreEqual("12345", option.Values[0]);
            Assert.AreEqual("7890", option.Values[1]);
        }

        [TestMethod]
        public void Parse_OptionWithRegexValidatorAndMultipleValues_UnSuccessful()
        {
            string testvalues = " --testoptionb:12345 --testoptionb:78b90";

            Command cmd = Command.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb", new RegularExpressionOptionValueValidator(@"^\d+$") { MaximumOccurrence = 2 });
            var option = cmd.Options["testoption"];

            bool error;

            cmd.Parse(testvalues, out error);
            Assert.IsTrue(error);
        }

        [TestMethod]
        public void Parse_OptionWithValueOptional_Successful()
        {
            string testvalues = " --testoptionb";

            Command cmd = Command.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb", new RegularExpressionOptionValueValidator(@"^\d+$") { ValueOptional = true });
            var option = cmd.Options["testoption"];

            bool error;

            cmd.Parse(testvalues, out error);
            Assert.IsFalse(error);
            Assert.IsTrue(option.IsSet);
        }

        [TestMethod]
        public void Parse_OptionWithValueOptionalTwiceTheSameOption_UnSuccessful()
        {
            string testvalues = " --testoptionb -testoption:123";

            Command cmd = Command.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb", new RegularExpressionOptionValueValidator(@"^\d+$") { ValueOptional = true });
            var option = cmd.Options["testoption"];

            bool error;

            cmd.Parse(testvalues, out error);
            Assert.IsTrue(error);
            Assert.IsFalse(option.IsSet);

            //------

            testvalues = " -testoption:123 --testoptionb";

            cmd = Command.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb", new RegularExpressionOptionValueValidator(@"^\d+$") { ValueOptional = true });
            option = cmd.Options["testoption"];

            cmd.Parse(testvalues, out error);
            Assert.IsTrue(error);
            Assert.IsFalse(option.IsSet);
        }

        [TestMethod]
        public void Parse_Argument_Successful()
        {
            string testvalues = " --testoptionb 123";
            bool error;

            Command cmd = Command.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument", new RegularExpressionArgumentValidator(@"^\d+$")).Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsFalse(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("123", argument.Value);
        }

        [TestMethod]
        public void Parse_ArgumentWithAmper_Successful()
        {
            string testvalues = " --testoptionb \"12 3\"";
            bool error;

            Command cmd = Command.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument", new RegularExpressionArgumentValidator(@"^\d+\s\d+$")).Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsFalse(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("12 3", argument.Value);
        }

        [TestMethod]
        public void Parse_ArgumentWithAmperMultipleValues_Successful()
        {
            string testvalues = " --testoptionb \"12 3\" \"456 65\"";
            bool error;

            Command cmd = Command.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument", new RegularExpressionArgumentValidator(@"^\d+\s\d+$") { MaximumOccurrence = 2 }).Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsFalse(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("12 3", argument.Values[0]);
            Assert.AreEqual("456 65", argument.Values[1]);
        }

        [TestMethod]
        public void Parse_ArgumentWithAmperMultipleValuesMaximumOccurrenceNotSet_UnSuccessful()
        {
            string testvalues = " --testoptionb \"12 3\" \"456 65\"";
            bool error;

            Command cmd = Command.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument",
                                                                   new RegularExpressionArgumentValidator(@"^\d+\s\d+$"))
                .Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsTrue(error);
            Assert.IsFalse(argument.IsSet);
        }

        [TestMethod]
        public void Parse_ArgumentCustomValidator_Successful()
        {
            string testvalues = " --testoptionb 123";
            bool error;

            Command cmd = Command.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument", new CustomArgumentValidator(s => s == "123")).Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsFalse(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("123", argument.Value);
        }

        [TestMethod]
        public void Parse_ArgumentCustomValidator_UnSuccessful()
        {
            string testvalues = " --testoptionb 123";
            bool error;

            Command cmd = Command.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument", new CustomArgumentValidator(s => s == "1234")).Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsTrue(error);
            Assert.IsFalse(argument.IsSet);
        }

        [TestMethod]
        public void Parse_DifferentOptionSplitter_Successful()
        {
            bool error;
            var option = Command.GetRoot().AddOption("test", "alpha;beta", new RegularExpressionOptionValueValidator(@"^\d+$") { MaximumOccurrence = 2 }).Parse(" -alpha:1 -beta=2", out error);

            Assert.AreEqual(2, option.Values.Length);
            Assert.AreEqual("1", option.Values[0]);
            Assert.AreEqual("2", option.Values[1]);
        }

        [TestMethod]
        public void Experimental()
        {

            bool error;


            Command.GetRoot().Add(cmd2 =>
            {
                cmd2.AddCommand("test", "test;uiu").Add(cmd3 =>
                {
                    cmd3.AddGlobalOption("glob", "yu;ty");
                });

                cmd2.AddOption("Iik", "dff").Parse(" tests", out error);


            });



            // Option.Create().

            // Command.Get().AddOption()

            //Command.GetRoot().AddOption("mike", "dfs").Parse().Values;




        }
    }
}
