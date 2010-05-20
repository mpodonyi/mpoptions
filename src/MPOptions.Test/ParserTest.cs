using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPOptions.Internal;

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
        public void Parse_ArgumentFallThroughValidator_Successful()
        {
            string testvalues = " --testoptionb 123 456";
            bool error;

            Command cmd = Command.GetRoot();
            var arg = cmd.AddOption("testoption", "testoptionb").AddArgument("testargument", new FallThroughArgumentValidator(){MaximumOccurrence = 2}).Parse(testvalues, out error);

            Assert.IsFalse(error);
            Assert.IsTrue(arg.IsSet);
            Assert.AreEqual("123",arg.Values[0]);
            Assert.AreEqual("456", arg.Values[1]);
        }

        [TestMethod]
        public void Parse_ArgumentFallThroughValidator_UnSuccessful()
        {
            string testvalues = " --testoptionb 123 456";
            bool error;

            Command cmd = Command.GetRoot();
            var arg = cmd.AddOption("testoption", "testoptionb").AddArgument("testargument", new FallThroughArgumentValidator()).Parse(testvalues, out error);
            Assert.IsTrue(error);
        }

        [TestMethod]
        public void Parse_OptionWithDifferentSplitter_Successful()
        {
            bool error;
            var option = Command.GetRoot().AddOption("test", "alpha;beta", new RegularExpressionOptionValueValidator(@"^\d+$") { MaximumOccurrence = 2 }).Parse(" -alpha:1 -beta=2", out error);

            Assert.AreEqual(2, option.Values.Length);
            Assert.AreEqual("1", option.Values[0]);
            Assert.AreEqual("2", option.Values[1]);
        }

        [TestMethod]
        public void Parse_OptionWithNonAlphaNumericToken_Successful()
        {
            bool error;
            var option = Command.GetRoot().AddOption("test", "?;@", new RegularExpressionOptionValueValidator(@"^\d+$") { MaximumOccurrence = 2 }).Parse(" -?:1 --@=2", out error);

            Assert.AreEqual(2, option.Values.Length);
            Assert.AreEqual("1", option.Values[0]);
            Assert.AreEqual("2", option.Values[1]);
        }

        [TestMethod]
        public void Parse_OptionWithFallThroughOptionValidator_Successful()
        {
            bool error;
            
            var option = Command.GetRoot().AddOption("test", "m", new FallThroughOptionValueValidator()).Parse(" -m:k", out error);
            Assert.IsTrue(option.IsSet);
            Assert.AreEqual("k", option.Values[0]);
            Assert.IsFalse(error);

            option = Command.GetRoot().AddOption("test", "m", new FallThroughOptionValueValidator(){MaximumOccurrence = 2}).Parse(" -m:k -m:l", out error);
            Assert.IsTrue(option.IsSet);
            Assert.AreEqual("k", option.Values[0]);
            Assert.AreEqual("l", option.Values[1]);
            Assert.IsFalse(error);
           
            option = Command.GetRoot().AddOption("test", "m", new FallThroughOptionValueValidator(){ValueOptional = true}).Parse(" -m", out error);
            Assert.IsTrue(option.IsSet);
            Assert.IsFalse(error);
        }

        [TestMethod]
        public void Parse_OptionWithFallThroughOptionValidator_UnSuccessful()
        {
            bool error;
            var option = Command.GetRoot().AddOption("test", "m", new FallThroughOptionValueValidator()).Parse(" -m:k -m:l", out error);
            Assert.IsTrue(error);

            option = Command.GetRoot().AddOption("test", "m", new FallThroughOptionValueValidator()).Parse(" -m", out error);
            Assert.IsTrue(error);

            option = Command.GetRoot().AddOption("test", "m", new FallThroughOptionValueValidator() { ValueOptional = true, MaximumOccurrence = 2 }).Parse(" -m -m", out error);
            Assert.IsTrue(error);

            option = Command.GetRoot().AddOption("test", "m", new FallThroughOptionValueValidator() { ValueOptional = true, MaximumOccurrence = 2 }).Parse(" -m -m:6", out error);
            Assert.IsTrue(error);
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
