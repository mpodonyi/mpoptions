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
            Command cmd = MPOptions.GetRoot();
            cmd.AddCommand("testcommand", "testcommanda; testcommandb");

            ParserErrorContext error;
            cmd.Parse(" testcommandb", out error);

            Assert.IsNull(error);
            Assert.IsTrue(cmd.Commands["testcommand"].IsSet);

        }

        [TestMethod]
        public void Parse_Command_Unsuccessful()
        {
            Command cmd = MPOptions.GetRoot();
            cmd.AddCommand("testcommand", "testcommanda; testcommandb");

            ParserErrorContext error;
            cmd.Parse(" testcommandc", out error);

            Assert.IsNotNull(error);
            Assert.IsFalse(cmd.Commands["testcommand"].IsSet);

        }

        [TestMethod]
        public void Parse_Option_Successful()
        {
            Command cmd = MPOptions.GetRoot();
            cmd.AddOption("testoption", "testoption; testoptionb");
            cmd.AddOption("testoption2", "testoptionc; testoptiond");
            cmd.AddOption("testoption3", "testoptione; testoptionf");

            ParserErrorContext error;
            cmd.Parse(@" --testoptionb /testoptionc -testoptione", out error);

            Assert.IsNull(error);
            Assert.IsTrue(cmd.Options["testoption"].IsSet);
            Assert.IsTrue(cmd.Options["testoption2"].IsSet);
            Assert.IsTrue(cmd.Options["testoption3"].IsSet);

        }

        [TestMethod]
        public void Parse_Option_Unsuccessful()
        {
            Command cmd = MPOptions.GetRoot();
            cmd.AddOption("testoption", "testoption; testoptionb");
            cmd.AddOption("testoption2", "testoptionc; testoptiond");
            cmd.AddOption("testoption3", "testoptione; testoptionf");

            ParserErrorContext error;
            cmd.Parse(@" --testoptionb /testoptionx -testoptione", out error);

            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void Parse_OptionWithStaticValidator_Successful()
        {
            Command cmd = MPOptions.GetRoot();
            cmd.AddOption("testoption", "testoption; testoptionb").WithStaticValidator("yes", "no");
            var option = cmd.Options["testoption"];

            ParserErrorContext error;
            //cmd.Parse(@" --testoptionb:yes \testoptionc -testoptione", out error);
            cmd.Parse(@" --testoptionb:yes", out error);

            Assert.IsNull(error);

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
                Command cmd = MPOptions.GetRoot();

                cmd.AddOption("testoption", "testoption; testoptionb").WithStaticValidator(testvalues[i, 0]);
                var option = cmd.Options["testoption"];

                ParserErrorContext error;

                cmd.Parse(testvalues[i, 1], out error);
                Assert.IsNull(error);
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
                Command cmd = MPOptions.GetRoot();

                cmd.AddOption("testoption", "testoption; testoptionb").WithRegexValidator(@"^\d+$");
                var option = cmd.Options["testoption"];

                ParserErrorContext error;

                cmd.Parse(testvalues[i, 1], out error);
                Assert.IsNull(error);
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
                Command cmd = MPOptions.GetRoot();

                cmd.AddOption("testoption", "testoption; testoptionb").WithRegexValidator(@"^\d+$");
                var option = cmd.Options["testoption"];

                ParserErrorContext error;

                cmd.Parse(testvalues[i, 1], out error);
                Assert.IsNotNull(error);
                Assert.IsFalse(option.IsSet);
                Assert.AreNotEqual(testvalues[i, 0], option.Value);
            }
        }

        [TestMethod]
        public void Parse_OptionWithRegexValidatorAndMultipleValues_Successful()
        {
            string testvalues = " --testoptionb:12345 --testoptionb:7890";

            Command cmd = MPOptions.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb").WithRegexValidator(@"^\d+$", 2 );
            var option = cmd.Options["testoption"];

            ParserErrorContext error;

            cmd.Parse(testvalues, out error);
            Assert.IsNull(error);
            Assert.IsTrue(option.IsSet);

            Assert.AreEqual("12345", option.Values[0]);
            Assert.AreEqual("7890", option.Values[1]);
        }

        [TestMethod]
        public void Parse_OptionWithRegexValidatorAndMultipleValues_UnSuccessful()
        {
            string testvalues = " --testoptionb:12345 --testoptionb:78b90";

            Command cmd = MPOptions.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb").WithRegexValidator(@"^\d+$",2 );
            var option = cmd.Options["testoption"];

            ParserErrorContext error;

            cmd.Parse(testvalues, out error);
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void Parse_OptionWithValueOptional_Successful()
        {
            string testvalues = " --testoptionb";

            Command cmd = MPOptions.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb").WithRegexValidator(@"^\d+$", true );
            var option = cmd.Options["testoption"];

            ParserErrorContext error;

            cmd.Parse(testvalues, out error);
            Assert.IsNull(error);
            Assert.IsTrue(option.IsSet);
        }

        [TestMethod]
        public void Parse_OptionWithValueOptionalTwiceTheSameOption_UnSuccessful()
        {
            string testvalues = " --testoptionb -testoption:123";

            Command cmd = MPOptions.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb").WithRegexValidator(@"^\d+$",true );
            var option = cmd.Options["testoption"];

            ParserErrorContext error;

            cmd.Parse(testvalues, out error);
            Assert.IsNotNull(error);
            Assert.IsFalse(option.IsSet);

            //------

            testvalues = " -testoption:123 --testoptionb";

            cmd = MPOptions.GetRoot();

            cmd.AddOption("testoption", "testoption; testoptionb").WithRegexValidator(@"^\d+$", true );
            option = cmd.Options["testoption"];

            cmd.Parse(testvalues, out error);
            Assert.IsNotNull(error);
            Assert.IsFalse(option.IsSet);
        }

        [TestMethod]
        public void Parse_Argument_Successful()
        {
            string testvalues = " --testoptionb 123";
            ParserErrorContext error;

            Command cmd = MPOptions.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument").WithRegexValidator(@"^\d+$").Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsNull(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("123", argument.Value);
        }

        [TestMethod]
        public void Parse_ArgumentWithAmper_Successful()
        {
            string testvalues = " --testoptionb \"12 3\"";
            ParserErrorContext error;

            Command cmd = MPOptions.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument").WithRegexValidator(@"^\d+\s\d+$").Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsNull(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("12 3", argument.Value);
        }

        [TestMethod]
        public void Parse_ArgumentWithAmperMultipleValues_Successful()
        {
            string testvalues = " --testoptionb \"12 3\" \"456 65\"";
            ParserErrorContext error;

            Command cmd = MPOptions.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument").WithRegexValidator(@"^\d+\s\d+$",2 ).Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

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

            Command cmd = MPOptions.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument").WithRegexValidator(@"^\d+\s\d+$").Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsNotNull(error);
            Assert.IsFalse(argument.IsSet);
        }

        [TestMethod]
        public void Parse_ArgumentCustomValidator_Successful()
        {
            string testvalues = " --testoptionb 123";
            ParserErrorContext error;

            Command cmd = MPOptions.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument").WithCustomValidator(s => s == "123").Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsNull(error);
            Assert.IsTrue(argument.IsSet);
            Assert.AreEqual("123", argument.Value);
        }

        [TestMethod]
        public void Parse_ArgumentCustomValidator_UnSuccessful()
        {
            string testvalues = " --testoptionb 123";
            ParserErrorContext error;

            Command cmd = MPOptions.GetRoot();
            cmd.AddOption("testoption", "testoptionb").AddArgument("testargument").WithCustomValidator(s => s == "1234").Parse(testvalues, out error);
            var argument = cmd.Arguments["testargument"];

            Assert.IsNotNull(error);
            Assert.IsFalse(argument.IsSet);
        }

        [TestMethod]
        public void Parse_ArgumentFallThroughValidator_Successful()
        {
            string testvalues = " --testoptionb 123 456";
            ParserErrorContext error;

            Command cmd = MPOptions.GetRoot();
            var arg = cmd.AddOption("testoption", "testoptionb").AddArgument("testargument",2).Parse(testvalues, out error);

            Assert.IsNull(error);
            Assert.IsTrue(arg.IsSet);
            Assert.AreEqual("123",arg.Values[0]);
            Assert.AreEqual("456", arg.Values[1]);
        }

        [TestMethod]
        public void Parse_ArgumentFallThroughValidator_UnSuccessful()
        {
            string testvalues = " --testoptionb 123 456";
            ParserErrorContext error;

            Command cmd = MPOptions.GetRoot();
            var arg = cmd.AddOption("testoption", "testoptionb").AddArgument("testargument").Parse(testvalues, out error);
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void Parse_OptionWithDifferentSplitter_Successful()
        {
            ParserErrorContext error;
            var option = MPOptions.GetRoot().AddOption("test", "alpha;beta").WithRegexValidator(@"^\d+$", 2 ).Parse(" -alpha:1 -beta=2", out error);

            Assert.AreEqual(2, option.Values.Length);
            Assert.AreEqual("1", option.Values[0]);
            Assert.AreEqual("2", option.Values[1]);
        }

        [TestMethod]
        public void Parse_OptionWithNonAlphaNumericToken_Successful()
        {
            ParserErrorContext error;
            var option = MPOptions.GetRoot().AddOption("test", "?;@").WithRegexValidator(@"^\d+$", 2 ).Parse(" -?:1 --@=2", out error);

            Assert.AreEqual(2, option.Values.Length);
            Assert.AreEqual("1", option.Values[0]);
            Assert.AreEqual("2", option.Values[1]);
        }

        [TestMethod]
        public void Parse_OptionWithFallThroughOptionValidator_Successful()
        {
            ParserErrorContext error;

            var option = MPOptions.GetRoot().AddOption("test", "m").WithNoValidator().Parse(" -m:k", out error);
            Assert.IsTrue(option.IsSet);
            Assert.AreEqual("k", option.Values[0]);
            Assert.IsNull(error);

            option = MPOptions.GetRoot().AddOption("test", "m").WithNoValidator(2).Parse(" -m:k -m:l", out error);
            Assert.IsTrue(option.IsSet);
            Assert.AreEqual("k", option.Values[0]);
            Assert.AreEqual("l", option.Values[1]);
            Assert.IsNull(error);

            option = MPOptions.GetRoot().AddOption("test", "m").WithNoValidator( true ).Parse(" -m", out error);
            Assert.IsTrue(option.IsSet);
            Assert.IsNull(error);
        }

        [TestMethod]
        public void Parse_OptionWithFallThroughOptionValidator_UnSuccessful()
        {
            ParserErrorContext error;
            var option = MPOptions.GetRoot().AddOption("test", "m").WithNoValidator().Parse(" -m:k -m:l", out error);
            Assert.IsNotNull(error);

            option = MPOptions.GetRoot().AddOption("test", "m").WithNoValidator().Parse(" -m", out error);
            Assert.IsNotNull(error);

            option = MPOptions.GetRoot().AddOption("test", "m").WithNoValidator(true, 2 ).Parse(" -m -m", out error);
            Assert.IsNotNull(error);

            option = MPOptions.GetRoot().AddOption("test", "m").WithNoValidator(true, 2 ).Parse(" -m -m:6", out error);
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void ToString_ParserErrorContextStringRepresentation_Successful()
        {
            ParserErrorContext error;
            MPOptions.GetRoot().AddOption("test", "m").WithStaticValidator("k").Parse(" -m:k -m:l", out error);

            Assert.AreEqual(" -m:k -m:l\r\n      ^", error.ToString());
        }

        [TestMethod]
        public void Parse_OnlyWhiteSpace_Successful()
        {
            ParserErrorContext error;
            MPOptions.GetRoot().AddOption("test", "m").Parse(" ", out error);
            Assert.IsNull(error);

            MPOptions.GetRoot().AddOption("test", "m").Parse("", out error);
            Assert.IsNull(error);
        }

        //=====================================================
        
        [TestMethod]
        public void Experimental()
        {

            ParserErrorContext error;


            MPOptions.GetRoot().Add(cmd2 =>
            {
                cmd2.AddCommand("test", "test;uiu").Add(cmd3 =>
                {
                    cmd3.AddGlobalOption("glob", "yu;ty");
                });

                cmd2.AddOption("Iik", "dff").Parse(" tests", out error);


            });



            // Option.Create().

            // Command.Get().AddOption()

            //MPOptions.GetRoot().AddOption("mike", "dfs").Parse().Values;




        }
    }
}
