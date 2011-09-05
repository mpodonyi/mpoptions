using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPOptions.Extensions;

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
            RootCommand cmd = MPOptions.GetRoot();
            cmd.Add(new Command("testcommand", "testcommanda; testcommandb"));

            ParserErrorContext error;
            ICommandResult result = cmd.Parse(" testcommandb", out error);

            Assert.IsNull(error);
            Assert.IsTrue(result.Commands["testcommand"].IsSet);

        }

        [TestMethod]
        public void Parse_Command_Unsuccessful()
        {
            RootCommand cmd = MPOptions.GetRoot();
            cmd.Add(new Command("testcommand", "testcommanda; testcommandb"));

            ParserErrorContext error;
            ICommandResult result = cmd.Parse(" testcommandc", out error);

            Assert.IsNotNull(error);
            Assert.IsFalse(result.Commands["testcommand"].IsSet);

        }

        [TestMethod]
        public void Parse_Option_Successful()
        {
            RootCommand cmd = MPOptions.GetRoot();
            cmd.Add(new Option("testoption", "testoption; testoptionb",false));
            cmd.Add(new Option("testoption2", "testoptionc; testoptiond",false));
            cmd.Add(new Option("testoption3", "testoptione; testoptionf",false));

            ParserErrorContext error;
            ICommandResult result = cmd.Parse(@" --testoptionb /testoptionc -testoptione", out error);

            Assert.IsNull(error);
            Assert.IsTrue(result.Options["testoption"].IsSet);
            Assert.IsTrue(result.Options["testoption2"].IsSet);
            Assert.IsTrue(result.Options["testoption3"].IsSet);
        }

        [TestMethod]
        public void Parse_Option_Unsuccessful()
        {
            RootCommand cmd = MPOptions.GetRoot();
            cmd.Add(new Option("testoption", "testoption; testoptionb",false));
            cmd.Add(new Option("testoption2", "testoptionc; testoptiond",false));
            cmd.Add(new Option("testoption3", "testoptione; testoptionf",false));

            ParserErrorContext error;
            ICommandResult result = cmd.Parse(@" --testoptionb /testoptionx -testoptione", out error);

            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void Parse_OptionWithStaticValidator_Successful()
        {
            RootCommand cmd = MPOptions.GetRoot();
            cmd.Add(new Option("testoption", "testoption; testoptionb",false).WithStaticValidator("yes", "no"));

            ParserErrorContext error;
            ICommandResult result = cmd.Parse(@" --testoptionb:yes", out error);

            Assert.IsNull(error);

            Assert.IsTrue(result.Options["testoption"].IsSet);
            Assert.AreEqual("yes", result.Options["testoption"].Value);
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
                RootCommand cmd = MPOptions.GetRoot();

                cmd.Add(new Option("testoption", "testoption; testoptionb",false).WithStaticValidator(testvalues[i, 0]));

                ParserErrorContext error;

                ICommandResult result = cmd.Parse(testvalues[i, 1], out error);
                Assert.IsNull(error);
                Assert.IsTrue(result.Options["testoption"].IsSet);
                Assert.AreEqual(testvalues[i, 0], result.Options["testoption"].Value);
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
                RootCommand cmd = MPOptions.GetRoot();

                cmd.Add(new Option("testoption", "testoption; testoptionb",false).WithRegexValidator(@"^\d+$"));
                //var option = cmd.Options["testoption"];

                ParserErrorContext error;

                ICommandResult result = cmd.Parse(testvalues[i, 1], out error);
                Assert.IsNull(error);
                Assert.IsTrue(result.Options["testoption"].IsSet);
                Assert.AreEqual(testvalues[i, 0], result.Options["testoption"].Value);
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
                RootCommand cmd = MPOptions.GetRoot();

                cmd.Add(new Option("testoption", "testoption; testoptionb",false).WithRegexValidator(@"^\d+$"));
                //var option = cmd.Options["testoption"];

                ParserErrorContext error;

                ICommandResult result = cmd.Parse(testvalues[i, 1], out error);
                Assert.IsNotNull(error);
                Assert.IsFalse(result.Options["testoption"].IsSet);
                Assert.AreNotEqual(testvalues[i, 0], result.Options["testoption"].Value);
            }
        }

        [TestMethod]
        public void Parse_OptionWithRegexValidatorAndMultipleValues_Successful()
        {
            string testvalues = " --testoptionb:12345 --testoptionb:7890";

            RootCommand cmd = MPOptions.GetRoot();

            cmd.Add(new Option("testoption", "testoption; testoptionb",false).WithRegexValidator(@"^\d+$", 2 ));
            //var option = cmd.Options["testoption"];

            ParserErrorContext error;

            ICommandResult result = cmd.Parse(testvalues, out error);
            Assert.IsNull(error);
            Assert.IsTrue(result.Options["testoption"].IsSet);

            Assert.AreEqual("12345", result.Options["testoption"].Values[0]);
            Assert.AreEqual("7890", result.Options["testoption"].Values[1]);
        }

        [TestMethod]
        public void Parse_OptionWithRegexValidatorAndMultipleValues_UnSuccessful()
        {
            string testvalues = " --testoptionb:12345 --testoptionb:78b90";

            RootCommand cmd = MPOptions.GetRoot();

            cmd.Add(new Option("testoption", "testoption; testoptionb",false).WithRegexValidator(@"^\d+$",2 ));
            //var option = cmd.Options["testoption"];

            ParserErrorContext error;

            ICommandResult result = cmd.Parse(testvalues, out error);
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void Parse_OptionWithValueOptional_Successful()
        {
            string testvalues = " --testoptionb";

            RootCommand cmd = MPOptions.GetRoot();

            cmd.Add(new Option("testoption", "testoption; testoptionb",false).WithRegexValidator(@"^\d+$", true ));
            //var option = cmd.Options["testoption"];

            ParserErrorContext error;

            ICommandResult result = cmd.Parse(testvalues, out error);
            Assert.IsNull(error);
            Assert.IsTrue(result.Options["testoption"].IsSet);
        }

        [TestMethod]
        public void Parse_OptionWithValueOptionalTwiceTheSameOption_UnSuccessful()
        {
            string testvalues = " --testoptionb -testoption:123";

            RootCommand cmd = MPOptions.GetRoot();

            cmd.Add(new Option("testoption", "testoption; testoptionb",false).WithRegexValidator(@"^\d+$",true ));
            //var option = cmd.Options["testoption"];

            ParserErrorContext error;

            ICommandResult result = cmd.Parse(testvalues, out error);
            Assert.IsNotNull(error);
            Assert.IsFalse(result.Options["testoption"].IsSet);

            //------

            testvalues = " -testoption:123 --testoptionb";

            cmd = MPOptions.GetRoot();

            cmd.Add(new Option("testoption", "testoption; testoptionb",false).WithRegexValidator(@"^\d+$", true ));
            //option = cmd.Options["testoption"];

            result = cmd.Parse(testvalues, out error);
            Assert.IsNotNull(error);
            Assert.IsFalse(result.Options["testoption"].IsSet);
        }

        [TestMethod]
        public void Parse_Argument_Successful()
        {
            string testvalues = " --testoptionb 123";
            ParserErrorContext error;

            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result = cmd.Add(new Option("testoption", "testoptionb",false)).Add(new Argument("testargument").WithRegexValidator(@"^\d+$")).Parse(testvalues, out error);
            var argument = result.Arguments["testargument"];

            Assert.IsNull(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("123", argument.Value);
        }

        [TestMethod]
        public void Parse_ArgumentWithAmper_Successful()
        {
            string testvalues = " --testoptionb \"12 3\"";
            ParserErrorContext error;

            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result = cmd.Add(new Option("testoption", "testoptionb", false)).Add(new Argument("testargument").WithRegexValidator(@"^\d+\s\d+$")).Parse(testvalues, out error);
            var argument = result.Arguments["testargument"];

            Assert.IsNull(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("12 3", argument.Value);
        }

        [TestMethod]
        public void Parse_ArgumentWithAmperMultipleValues_Successful()
        {
            string testvalues = " --testoptionb \"12 3\" \"456 65\"";
            ParserErrorContext error;

            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result = cmd.Add(new Option("testoption", "testoptionb",false)).Add(new Argument("testargument").WithRegexValidator(@"^\d+\s\d+$").SetMaximumOccurrence(2)).Parse(testvalues, out error);
            var argument = result.Arguments["testargument"];

            Assert.IsNull(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("12 3", argument.Values[0]);
            Assert.AreEqual("456 65", argument.Values[1]);
        }

        [TestMethod]
        public void Parse_ArgumentWithAmperMultipleValuesMaximumOccurrenceNotSet_UnSuccessful()
        {
            string testvalues = " --testoptionb \"12 3\" \"456 65\"";
            ParserErrorContext error;

            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result = cmd.Add(new Option("testoption", "testoptionb",false)).Add(new Argument("testargument").WithRegexValidator(@"^\d+\s\d+$")).Parse(testvalues, out error);
            var argument = result.Arguments["testargument"];

            Assert.IsNotNull(error);
            Assert.IsFalse(argument.IsSet);
        }

        [TestMethod]
        public void Parse_ArgumentCustomValidator_Successful()
        {
            string testvalues = " --testoptionb 123";
            ParserErrorContext error;

            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result = cmd.Add(new Option("testoption", "testoptionb",false)).Add(new Argument("testargument").WithCustomValidator(s => s == "123")).Parse(testvalues, out error);
            var argument = result.Arguments["testargument"];

            Assert.IsNull(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("123", argument.Value);
        }

        [TestMethod]
        public void Parse_ArgumentCustomValidator_UnSuccessful()
        {
            string testvalues = " --testoptionb 123";
            ParserErrorContext error;

            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result = cmd.Add(new Option("testoption", "testoptionb",false)).Add(new Argument("testargument").WithCustomValidator(s => s == "1234")).Parse(testvalues, out error);
            var argument = result.Arguments["testargument"];

            Assert.IsNotNull(error);
            Assert.IsFalse(argument.IsSet);
        }

        [TestMethod]
        public void Parse_ArgumentFallThroughValidator_Successful()
        {
            string testvalues = " --testoptionb 123 456";
            ParserErrorContext error;

            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result =  cmd.Add(new Option("testoption", "testoptionb",false)).Add(new Argument("testargument").WithNoValidator().SetMaximumOccurrence(2)).Parse(testvalues, out error);
            var argument = result.Arguments["testargument"];

            Assert.IsNull(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("123", argument.Values[0]);
            Assert.AreEqual("456", argument.Values[1]);
        }

        [TestMethod]
        public void Parse_ArgumentFallThroughValidator_UnSuccessful()
        {
            string testvalues = " --testoptionb 123 456";
            ParserErrorContext error;

            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result = cmd.Add(new Option("testoption", "testoptionb",false)).Add(new Argument("testargument")).Parse(testvalues, out error);
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void Parse_OptionWithDifferentSplitter_Successful()
        {
            ParserErrorContext error;

            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result = cmd.Add(new Option("test", "alpha;beta", false).WithRegexValidator(@"^\d+$", 2)).Parse(" -alpha:1 -beta=2", out error);
            var option = result.Options["test"];

            Assert.AreEqual(2, option.Values.Length);
            Assert.AreEqual("1", option.Values[0]);
            Assert.AreEqual("2", option.Values[1]);
        }

        [TestMethod]
        public void Parse_OptionWithNonAlphaNumericToken_Successful()
        {
            ParserErrorContext error;
            
            RootCommand cmd = MPOptions.GetRoot();
            ICommandResult result = cmd.Add(new Option("test", "?;@",false).WithRegexValidator(@"^\d+$", 2)).Parse(" -?:1 --@=2", out error);
            var option = result.Options["test"];

            Assert.AreEqual(2, option.Values.Length);
            Assert.AreEqual("1", option.Values[0]);
            Assert.AreEqual("2", option.Values[1]);
        }

        [TestMethod]
        public void Parse_OptionWithFallThroughOptionValidator_Successful()
        {
            ParserErrorContext error;
            RootCommand cmd = MPOptions.GetRoot();
            
            ICommandResult result = cmd.Add(new Option("test", "m", false).WithNoValidator()).Parse(" -m:k", out error);
            var option = result.Options["test"];
            Assert.IsTrue(option.IsSet);
            Assert.AreEqual("k", option.Values[0]);
            Assert.IsNull(error);

            cmd = MPOptions.GetRoot();
            result = cmd.Add(new Option("test", "m", false).WithNoValidator(true)).Parse(" -m:k", out error);
            option = result.Options["test"];
            Assert.IsTrue(option.IsSet);
            Assert.AreEqual("k", option.Values[0]);
            Assert.IsNull(error);

            cmd = MPOptions.GetRoot();
            result = cmd.Add(new Option("test", "m", false).WithNoValidator(2)).Parse(" -m:k -m:l", out error);
            option = result.Options["test"];
            Assert.IsTrue(option.IsSet);
            Assert.AreEqual("k", option.Values[0]);
            Assert.AreEqual("l", option.Values[1]);
            Assert.IsNull(error);

            cmd = MPOptions.GetRoot();
            result = cmd.Add(new Option("test", "m", false).WithNoValidator(true, 2)).Parse(" -m:k -m:l", out error);
            option = result.Options["test"];
            Assert.IsTrue(option.IsSet);
            Assert.AreEqual("k", option.Values[0]);
            Assert.AreEqual("l", option.Values[1]);
            Assert.IsNull(error);


            cmd = MPOptions.GetRoot();
            result = cmd.Add(new Option("test", "m",false).WithNoValidator( true )).Parse(" -m", out error);
            option = result.Options["test"];
            Assert.IsTrue(option.IsSet);
            Assert.IsNull(error);
        }

        [TestMethod]
        public void Parse_OptionWithFallThroughOptionValidator_UnSuccessful()
        {
            ParserErrorContext error;
            RootCommand cmd = MPOptions.GetRoot();

            ICommandResult result = cmd.Add(new Option("test", "m",false).WithNoValidator()).Parse(" -m:k -m:l", out error);
            Assert.IsNotNull(error);

            cmd = MPOptions.GetRoot();
            result = cmd.Add(new Option("test", "m",false).WithNoValidator()).Parse(" -m", out error);
            Assert.IsNotNull(error);

            cmd = MPOptions.GetRoot();
            result = cmd.Add(new Option("test", "m",false).WithNoValidator(true, 2)).Parse(" -m -m", out error);
            Assert.IsNotNull(error);

            cmd = MPOptions.GetRoot();
            result = cmd.Add(new Option("test", "m",false).WithNoValidator(true, 2)).Parse(" -m -m:6", out error);
            Assert.IsNotNull(error);

            cmd = MPOptions.GetRoot();
            result = cmd.Add(new Option("test", "m", false).WithNoValidator(true, 2)).Parse(" -m:6 -m", out error);
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void ToString_ParserErrorContextStringRepresentation_Successful()
        {
            ParserErrorContext error;
            RootCommand cmd = MPOptions.GetRoot();
            
            ICommandResult result = cmd.Add(new Option("test", "m",false).WithStaticValidator("k")).Parse(" -m:k -m:l", out error);

            Assert.AreEqual(" -m:k -m:l\r\n      ^", error.ToString());
        }

        [TestMethod]
        public void Parse_OnlyWhiteSpace_Successful()
        {
            ParserErrorContext error;
            RootCommand cmd = MPOptions.GetRoot();

            ICommandResult result = cmd.Add(new Option("test", "m",false)).Parse(" ", out error);
            Assert.IsNull(error);

            cmd = MPOptions.GetRoot();
            result = cmd.Add(new Option("test", "m",false)).Parse("", out error);
            Assert.IsNull(error);
        }

      
    }
}
