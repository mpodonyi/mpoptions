#region Copyright 2009 by Mike Podonyi, Licensed under the GNU Lesser General Public License
/*  This file is part of Console.Net.

 *  Console.Net is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.

 *  Console.Net is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.

 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Console.Net.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using MPOptions.Internal;
using MPOptions.NewStyle;


namespace MPOptions
{

    public interface IGeneralFlow<T> where T : Command
    {
        T Add(params Option[] options);

        T Add(params Command[] commands);

        T Add(Argument argument);
    }


    public static class MPOptions
    {
        public static RootCommand GetRoot()
        {
            return new RootCommand();
        }
    }


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
            var result = new CommandResult(this);
            var parser = new Parser(result, commandLine);
            parserErrorContext = parser.Parse();
            //return this;
            return result;
        }

        
    }

    public class ElementCollection<T> : KeyedCollection<string, T> where T : Element
    {

        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }
    }

    
}