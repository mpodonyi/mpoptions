using MPOptions.Internal;
using MPOptions.NewStyle;

namespace MPOptions
{
    public class RootCommand : Command, IGeneralFlow<RootCommand>
    {
        internal RootCommand():base("","")
        {
            // this.StateBag=new StateBag {RootCommand = this};
        }



        public new RootCommand Add(params Command[] commands) //MP: could be an extension method
        {
            base.Add(commands);
            return this;
        }

        //MP: could be an extension method
        //MP: maybe derive a class GlobalOption from Option
        public new RootCommand Add(params Option[] options) 
        {
            base.Add(options);
            return this;
        }

        public new RootCommand Add(Argument argument)
        {
            base.Add(argument);
            return this;
        }

      

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Parses this instance.
        /// </summary>
        /// <exception cref="ParserException">Thrown when the Commandline can not be parsed successful.</exception>
        /// <returns>Return the current command.</returns>
        public Command Parse()
        {
            var parser = new Parser(this);
            ParserErrorContext errorContext = parser.Parse();
            if (errorContext != null)
                ThrowHelper.ThrowParserException(errorContext);
            return this;
        }

        ///// <summary>
        ///// Parses this instance.
        ///// </summary>
        ///// <param name="parserErrorContext">The Error Reported by the Parser. Otherwise Null.</param>
        ///// <returns>Return the current command.</returns>
        //public Command Parse(out ParserErrorContext parserErrorContext)
        //{
        //    var parser = new Parser(this);
        //    parserErrorContext = parser.Parse();
        //    return this;
        //}


        internal ICommandResult Parse(string commandLine, out ParserErrorContext parserErrorContext)
        {
            var result = new CommandResult(this,new ResultStateBag());
            var parser = new Parser(result, commandLine);
            parserErrorContext = parser.Parse();
            //return this;
            return result;
        }

        
    }
}