using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPOptions.Extensions;

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
            Command root = MPOptions.GetRoot();
            Assert.IsTrue(root.IsRoot);
        }


        [TestMethod]
        [Ignore]
        public void AddCommand_AddsNewCommand_ReturnsCurrentCommand()
        {
            //RootCommand rootcmd = MPOptions.GetRoot().Add(new Command("test","testtoken"));
            //Command cmd = rootcmd.Commands["test"];
              
            //Assert.AreEqual("test", cmd.Name);
            //Assert.AreEqual("testtoken", cmd.Token);
            //Assert.IsFalse(cmd.IsRoot);
            //Assert.IsTrue(cmd.ParentCommand.IsRoot);
            //Assert.IsTrue(cmd.RootCommand.IsRoot);
        }

        [TestMethod]
        public void CommandsIndexer_GetChildCommandByName_ReturnsSingleChildCommand()
        {
            Command newcommand = new Command("test", "testtoken");
            RootCommand cmd = MPOptions.GetRoot().Add(newcommand);
            Assert.AreSame(newcommand, cmd.Commands["test"]);
        }

        [TestMethod]
        public void CommandsEnumerator_Execute_ReturnsAllChildCommand()
        {
            RootCommand root = MPOptions.GetRoot();
            Command cmd1 = new Command("test1", "testtoken1");
            Command cmd2 = new Command("test2", "testtoken2");
            root.Add(cmd1, cmd2);

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
                string token = validToken;
                AssertHelper.ThrowsNoException(() => MPOptions.GetRoot().Add(new Command("test", token)));
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
                string token = invalidToken;
                AssertHelper.Throws<ArgumentException>(() => MPOptions.GetRoot().Add(new Command("test", token)));
            }
        }

        [TestMethod]
        public void AddCommand_DuplicateToken_ThrowsException()
        {
            RootCommand cmd = MPOptions.GetRoot();

            cmd.Add(new Command("test", "mike;was;here"));
            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Command("test2", "john;was;not")));
        }

        [TestMethod]
        public void AddCommand_DuplicateName_ThrowsException()
        {
            RootCommand cmd = MPOptions.GetRoot();

            cmd.Add(new Command("test", "mike;was;here"));
            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Command("test", "john;wasnt;not")));
        }

        [TestMethod]
        [Ignore]
        public void AddCommand_TestHierarchy_ReturnsCommandsInHierarchy()
        {
            //Command cmd = MPOptions.GetRoot().AddCommand("test1", "mike;was;here").AddCommand("test", "mike;was;here");

            //Assert.AreEqual("test", cmd.Name);
            //Assert.AreEqual("test1", cmd.ParentCommand.Name);
            //Assert.IsTrue(cmd.ParentCommand.ParentCommand.IsRoot);
        }

        [TestMethod]
        public void AddGlobalOption_Execute_ReturnsGlobalOption()
        {
            RootCommand cmd = MPOptions.GetRoot().Add(new Option("globop","s;k",true));
            Assert.IsTrue(cmd.Options["globop"].IsGlobalOption);

        }

        [TestMethod]
        public void AddGlobalOption_AddedGlobalOptionIsGivenBackInWholeHierarchy_ReturnCommand()
        {
            RootCommand cmd = MPOptions.GetRoot().Add(
                new Option("globop", "s;k", true)
                )
                .Add(
                    new Command("mike", "fire").Add(
                        new Option("locop", "v;l", false))
                );
          
            Assert.IsTrue(cmd.IsRoot);
            //Assert.IsNotNull(cmd.Options["locop"]);
            Assert.IsTrue(cmd.Commands["mike"].Options["globop"].IsGlobalOption);
            Assert.IsFalse(cmd.Commands["mike"].Options["locop"].IsGlobalOption);
            Assert.AreEqual(2, cmd.Commands["mike"].Options.Count);

            //Command cmd = MPOptions.GetRoot().AddGlobalOption("globop", "s;k").AddCommand("mike", "fire").AddOption("locop", "v;l").ParentCommand;
            //Assert.IsFalse(cmd.IsRoot);
            //Assert.IsNotNull(cmd.Options["locop"]);
            //Assert.IsTrue(cmd.Options["globop"].IsGlobalOption);
            //Assert.AreEqual(2, cmd.Options.Count);
        }

        [TestMethod]
        public void AddOption_Execute_ReturnsOption()
        {
            RootCommand root = MPOptions.GetRoot().Add(new Option("globop", "s;k", false));

            //Command cmd = MPOptions.GetRoot().AddOption("globop", "s;k").ParentCommand;
            Assert.IsNotNull(root.Options["globop"]);

        }

        [TestMethod]
        public void AddOption_NameAlreadyNameOfGlobalOption_ThrowsException()
        {
            Command cmd = MPOptions.GetRoot().Add(new Option("mike", "t1",true));
            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike", "t2",false)));

            cmd = MPOptions.GetRoot().Add(new Option("mike", "t1",false));
            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike", "t2",true)));

            //Command cmd = MPOptions.GetRoot();
            //cmd.AddGlobalOption("mike", "t1");
            //AssertHelper.Throws<ArgumentException>(() => cmd.AddOption("mike", "t2"));

            //cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t1");
            //AssertHelper.Throws<ArgumentException>(() => cmd.AddGlobalOption("mike", "t2"));
        }


        [TestMethod]
        public void AddOption_TokenAlreadyTokenOfGlobalOption_ThrowsException()
        {
            Command cmd = MPOptions.GetRoot().Add(new Option("mike", "t1",true));
            
            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike1", "t1",false)));

            //Command cmd = MPOptions.GetRoot();
            //cmd.AddGlobalOption("mike", "t1").AddOption("mike1", "t1");

            //ParserErrorContext error=null;
            //AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1",out error));
            //Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_OptionWithRegularExpressionAlreadyExist_ThrowsException()
        {
            Command cmd = MPOptions.GetRoot().Add(new Option("mike", "t1", false).WithRegexValidator(@"\d"));

            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike1", "t1", false).WithRegexValidator(@"\d")));

            //Command cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t1").WithRegexValidator(@"\d").AddOption("mike1", "t1").WithRegexValidator(@"\d");

            //ParserErrorContext error = null;
            //AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1", out error));
            //Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_OptionWhereStaticValueOfStaticValueValidatorExistAlready_ThrowsException()
        {
            Command cmd = MPOptions.GetRoot().Add(new Option("mike", "t1",false).WithStaticValidator("mike", "was"));

            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike1", "t1",false).WithStaticValidator("john", "was")));
            
            //Command cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t1").WithStaticValidator("mike", "was").AddOption("mike1", "t1").WithStaticValidator("john", "was");

            //ParserErrorContext error = null;
            //AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1:was",out error));
            //Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_OptionWhereStaticValueOfStaticValueValidatorDoesNotExistForOptionWithSameToken_Successful()
        {
            Command cmd = MPOptions.GetRoot().Add(new Option("mike", "t1",false).WithStaticValidator("mike", "was"));

            AssertHelper.ThrowsNoException(() => cmd.Add(new Option("mike1", "t1",false).WithStaticValidator("john", "not")));

            //Command cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t1").WithStaticValidator("mike", "was").AddOption("mike1", "t1").WithStaticValidator("john", "not");

            //ParserErrorContext error = null;
            //AssertHelper.ThrowsNoException(() => cmd.Parse(" -t1:was", out error));
            //Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_2OptionsWithSameValueBut1WithValidatorand1Without_ThrowsException()
        {
            Command cmd;

            cmd = MPOptions.GetRoot().Add(new Option("mike", "t1",false));
            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike1", "t1",false).WithStaticValidator("john", "was")));

            cmd = MPOptions.GetRoot().Add(new Option("mike", "t1",false));
            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike2", "t1", false).WithRegexValidator("\\d")));

            //Command cmd;
            //ParserErrorContext error = null;

            //cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t1").AddOption("mike1", "t1").WithStaticValidator("john", "was");
            //AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1", out error));
            //Assert.IsNull(error);

            //cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t1").AddOption("mike2", "t1").WithRegexValidator("\\d");
            //AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1", out error));
            //Assert.IsNull(error);
       }

        [TestMethod]
        public void AddOption_2OptionsWithSameValueAndNoValidator_ThrowsException()
        {
            Command cmd = MPOptions.GetRoot().Add(new Option("mike", "t1;t2;t3",false));

            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike1", "t4;t2;t5",false)));

            //Command cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t1;t2;t3").AddOption("mike1", "t4;t2;t5");

            //ParserErrorContext error = null;
            //AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1", out error));
            //Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_2OptionsWithTokenAndValueOptional_ThrowsException()
        {
            Command cmd = MPOptions.GetRoot().Add(new Option("mike", "t1;t2;t3",false).WithStaticValidator(true, "x", "y"));

            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike2", "t4;t2;t5", false).WithRegexValidator(@"^\d+$", true)));

            cmd = MPOptions.GetRoot().Add(new Option("mike", "t1;t2;t3",false));

            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike2", "t4;t2;t5", false).WithRegexValidator(@"^\d+$", true)));

            //cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t1;t2;t3").WithStaticValidator("x", "y").AddOption("mike2", "t4;t2;t5").WithRegexValidator(@"^\d+$", true);

            //AssertHelper.ThrowsNoException(() => cmd.Parse(" -t1:x",out error));
            //Assert.IsNull(error);
        }

        [TestMethod]
        public void AddOption_InvalidToken_ThrowsException()
        {
            AssertHelper.Throws<ArgumentException>(() => MPOptions.GetRoot().Add(new Option("mike", "-test",false)));
            AssertHelper.Throws<ArgumentException>(() => MPOptions.GetRoot().Add(new Option("mike", "te st",false)));
            AssertHelper.Throws<ArgumentException>(() => MPOptions.GetRoot().Add(new Option("mike", "test=",false)));
            AssertHelper.Throws<ArgumentException>(() => MPOptions.GetRoot().Add(new Option("mike", "test:",false)));

            AssertHelper.ThrowsNoException(() => MPOptions.GetRoot().Add(new Option("mike", "?",false)));
            AssertHelper.ThrowsNoException(() => MPOptions.GetRoot().Add(new Option("mike", " ? ",false)));
            AssertHelper.ThrowsNoException(() => MPOptions.GetRoot().Add(new Option("mike", " te?st",false)));
            AssertHelper.ThrowsNoException(() => MPOptions.GetRoot().Add(new Option("mike", "/",false)));
        }

        [TestMethod]
        public void AddGlobalOption_GlobalOptionWithNameOfLocalOption_ThrowsException()
        {
            Command cmd = MPOptions.GetRoot().Add(new Command("mikecom", "test").Add(new Option("mikeopt", "t1;t2;t3", false)));
            //cmd = cmd.AddCommand("mikecom", "test").AddOption("mikeopt", "t1;t2;t3").RootCommand;

            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mikeopt", "t4", true)));
        }

        [TestMethod]
        public void AddGlobalOption_GlobalOptionWithTokenOfLocalOption_ThrowsException()
        {
            RootCommand cmd = MPOptions.GetRoot();
            cmd.Add(new Command("mikecom", "test").Add(new Option("mikeopt", "t1;t2;t3",false)));
            
            

            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mikeglobopt", "t4;t2;t3",true)));
        
            //Command cmd = MPOptions.GetRoot();
            //cmd.AddCommand("mikecom", "test").AddOption("mikeopt", "t1;t2;t3").RootCommand.AddGlobalOption("mikeglobopt", "t4;t2;t3");

            //ParserErrorContext error = null;
            //AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1",out error));
            //Assert.IsNull(error);
        }

        [TestMethod]
        public void AddArgument_Execute_ReturnsArgument()
        {
            RootCommand cmd = MPOptions.GetRoot().Add(
                new Argument("arg").WithRegexValidator(@"^\d+$")
                );
            Assert.IsNotNull(cmd.Arguments["arg"]);
            //Command cmd = MPOptions.GetRoot().AddArgument("arg").WithRegexValidator(@"^\d+$").ParentCommand;
            //Assert.IsNotNull(cmd.Arguments["arg"]);
        }

        [TestMethod]
        public void AddArgument_MoreThenOneArgumentForCommand_ThrowsException()
        {
            RootCommand cmd = MPOptions.GetRoot().Add(new Argument("arg"));
            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Argument("arg2")));


            //var cmd = MPOptions.GetRoot().AddArgument("arg");
            //AssertHelper.Throws<ArgumentException>(()=>cmd.AddArgument("arg2"));
        }

        [TestMethod]
        public void AddOption_AfterGlobalOptionInFluentStyleAddsOptionToParentCommand_Successful()
        {
            RootCommand rootcommand = MPOptions.GetRoot();

            //Adds Option to command test
            AssertHelper.ThrowsNoException(() =>
            {
                rootcommand.Add(
                    new Command("test", "mike").Add(
                        new Option("globopt", "gl", true),
                        new Option("option", "opt", false)
                        )
                    );
            });

            Assert.IsNotNull(rootcommand.Commands["test"].Options["option"]);
            //Assert.IsNotNull(rootcommand.Commands["test"].Options["globopt"]);
            //Assert.IsNotNull(rootcommand.Options["globopt"]);
            
            //MP: reenable ParentCommand with newstyle
            //Assert.AreEqual(rootcommand.Commands["test"], rootcommand.Commands["test"].Options["option"].ParentCommand);

            //-------------------------------------------
            
            //Command rootcommand = MPOptions.GetRoot();

            ////Adds Option to command test
            //AssertHelper.ThrowsNoException(() =>
            //{
            //    rootcommand.AddCommand("test", "mike").AddGlobalOption("globopt", "gl").AddOption("option", "opt");
            //});

            //Assert.IsNotNull(rootcommand.Commands["test"].Options["option"]);

            //Assert.AreEqual(rootcommand.Commands["test"], rootcommand.Commands["test"].Options["option"].ParentCommand);

        }

        [TestMethod]
        [Ignore]
        public void AddOption_AfterGlobalOptionGetByIndexerAddsOptionToContextParentCommand_Successful()
        {
            //RootCommand rootcommand = MPOptions.GetRoot();

            //rootcommand.AddCommand("test", "mike").AddGlobalOption("globopt", "gl");

            //AssertHelper.ThrowsNoException(() =>
            //{
            //    rootcommand.Commands["test"].Options["globopt"].AddOption("opt2", "opt2");
            //});

            //Assert.IsNotNull(rootcommand.Commands["test"].Options["opt2"]);

            //Assert.AreEqual(rootcommand.Commands["test"], rootcommand.Commands["test"].Options["opt2"].ParentCommand);

            //Option option = rootcommand.Commands["test"].Options["globopt"];

            //Assert.IsTrue(option.IsGlobalOption);

            //Assert.AreEqual(rootcommand, option.ParentCommand);
        }

        [TestMethod]
        [Ignore]
        public void AddOption_OverSavedInstanceOfGlobalOptionAddsToContextCommand_Successful()
        {
            Command rootcommand = MPOptions.GetRoot();

            rootcommand.Add(new Command("com1", "com")).Add(new Command("com2", "com").Add(new Option("globopt", "gopt",true)));

            Option option1 = rootcommand.Commands["com1"].Options["globopt"];

            Option option2 = rootcommand.Commands["com1"].Commands["com2"].Options["globopt"];

            //option1.AddOption("option", "opt");

            //option2.AddOption("option2", "opt");

            Assert.IsNotNull(rootcommand.Commands["com1"].Options["option"]);

            Assert.IsNotNull(rootcommand.Commands["com1"].Commands["com2"].Options["option2"]);
        }

        [TestMethod]
        public void AddOption_OptionWithValidatorHasAlreadyOtherOptionsWithValidatorWithSameTokenValue_ThrowsException()
        {
            RootCommand cmd = MPOptions.GetRoot();
            cmd.Add(new Option("mike", "t2", false).WithRegexValidator(@"\d"));
            AssertHelper.ThrowsNoException(() => cmd.Add(new Option("mike1", "t1",false).WithNoValidator()));

            cmd = MPOptions.GetRoot();
            cmd.Add(new Option("mike", "t1",false).WithRegexValidator(@"\d"));
            AssertHelper.Throws<ArgumentException>(() => cmd.Add(new Option("mike1", "t1",false).WithNoValidator()));

            //ParserErrorContext error = null;
            //Command cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t2").WithRegexValidator(@"\d").AddOption("mike1", "t1").WithNoValidator();
            //AssertHelper.ThrowsNoException(() => cmd.Parse(" -t1:y7",out error));
            //Assert.IsNull(error);

            //cmd = MPOptions.GetRoot();
            //cmd.AddOption("mike", "t1").WithRegexValidator(@"\d").AddOption("mike1", "t1").WithNoValidator();
            //AssertHelper.Throws<ArgumentException>(() => cmd.Parse(" -t1:y7", out error));
            //Assert.IsNull(error);
        }


        [TestMethod]
        public void Element_ChangeOfElementPropertiesAfterAddingToCommandTree_ThrowsException()
        {
            RootCommand cmd = MPOptions.GetRoot();
            Option opt = new Option("mike", "t1", false);
            Argument arg=new Argument("arg");

            cmd.Add(opt).Add(arg);
            
            AssertHelper.Throws<InvalidOperationException>(() => opt.WithStaticValidator("foo","bar"));
            AssertHelper.Throws<InvalidOperationException>(() => arg.WithNoValidator());
        }
    }
}
