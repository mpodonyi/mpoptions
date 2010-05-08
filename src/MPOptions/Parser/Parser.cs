using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Internal;

namespace MPOptions.Parser
{
    internal class Parser
    {
        private const char OptionStarter = '-';
        private const char OptionStarter2 = '/';
        private const char OptionDivider = ':';
        private const char OptionDivider2 = '=';



        
        private enum Token
        {
            AlphaNumeric = 0x1,
            OptionDivider= 0x2,
            OptionStarter = 0x4,
            AmperAmper = 0x8,
            WhiteSpace = 0x10,
            End = 0x20,
            Error = 0x40,
            Amper = 0x80,
            OptionStarter2 = 0x100
        }

        private char[] charArray = null;
        private bool error = false;

        private Command currentCommand;

        private Token ParseValue(int pos)
        {
            if (pos >= charArray.Length)
                return Token.End;

            char testvalue = charArray[pos];
            switch (testvalue )
            {
                case OptionStarter:
                    return Token.OptionStarter;
                case OptionStarter2:
                    return Token.OptionStarter2;
                case '\\':
                    return Token.AmperAmper;
                case OptionDivider:
                case OptionDivider2:
                    return Token.OptionDivider;
                case '"':
                    return Token.Amper;
                default:
                    if(char.IsWhiteSpace(testvalue ))
                        return Token.WhiteSpace;
                    else if(char.IsLetterOrDigit(testvalue ))
                        return Token.AlphaNumeric;
                    else
                    {
                        return Token.Error;
                    }
            }

        }


        internal Parser(Command rootCommand, string commandLine)
        {
            charArray = commandLine.ToCharArray();
            currentCommand = rootCommand;
        }

        private int SwallowExe()
        {
            int retval = Environment.GetCommandLineArgs()[0].Length;
            return charArray[0] == '"' ? retval + 2 : retval;
        }

        private void Clean()
        {
            foreach (var value in currentCommand.StateBag.Options.Values)
            {
                value.Set = false;
                value._Values.Clear();
            }

            foreach (var value in currentCommand.StateBag.Commands.Values)
            {
                value.Set = false;
            }

            foreach (var value in currentCommand.StateBag.Arguments.Values)
            {
                value.Set = false;
            }
        }

        internal bool Parse()
        {
            Clean();
            //Parse(SwallowExe());
            Parse(0);

            if(error)
                Clean();
            
            return error;
        }

        private void Parse(int pos)
        {
            while(ParseValue(pos) != Token.End && !error)
            {
                while (ParseValue(pos) == Token.WhiteSpace)
                    pos++;

                if (ParseValue(pos) == Token.AlphaNumeric)
                {
                    if (TryCommand(ref pos))
                        continue;
                }
                else if (ParseValue(pos) == Token.OptionStarter || ParseValue(pos) == Token.OptionStarter2)
                {
                    if (TryOption(ref pos))
                        continue;
                }
                
                if (ParseValue(pos) != Token.End)
                {
                    if (TryArgument(ref pos))
                        continue;
                }

                error = true;

            }
        }

        private bool TryArgument(ref int pos)
        {
            int savedpos = pos;

            if (ParseValue(savedpos) == Token.Amper)
            {
                StringBuilder sb = new StringBuilder(50);

                if (ParseAmperString(sb, ref savedpos) && sb.Length > 0)
                {
                    if (TestForArgument(sb.ToString()))
                    {
                        pos = savedpos;
                        return true;
                    }
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder(50);

                do
                {
                    sb.Append(charArray[savedpos]);
                    savedpos++;
                } while (ParseValue(savedpos) != Token.WhiteSpace && ParseValue(savedpos) != Token.End);

                if (TestForArgument(sb.ToString()))
                {
                    pos = savedpos;
                    return true;
                }


            }

            return false;
        }

        private bool TestForArgument(string value)
        {
            var arguments = (from obj in currentCommand.Arguments
                             where obj._Values.Count < obj.ArgumentValidator.MaximumOccurrence
                                   && obj.ArgumentValidator is RegularExpressionArgumentValidator
                             select obj).Concat(
                                    from obj in currentCommand.Arguments
                                    where obj._Values.Count < obj.ArgumentValidator.MaximumOccurrence
                                          && obj.ArgumentValidator is CustomArgumentValidator
                                    select obj);

            foreach(var argument in arguments)
            {
                if(argument.ArgumentValidator.IsMatch(value))
                {
                    argument._Values.Add(value);
                    argument.Set = true;
                    return true;
                }
            }
           
            return false;
        }

        private bool ParseAmperString(StringBuilder retval, ref int pos)
        {
            while (true)
            {
                pos++;

                if (ParseValue(pos) == Token.Amper)
                {
                    pos++;
                    return true;
                }
                else if (ParseValue(pos) == Token.AmperAmper && ParseValue(pos + 1) == Token.Amper)
                {
                    pos++;
                    retval.Append('"');
                }
                else if (ParseValue(pos) == Token.AmperAmper && ParseValue(pos + 1) == Token.AmperAmper)
                {
                    pos++;
                    retval.Append('\\');
                }
                else if (ParseValue(pos) == Token.End)
                {
                    return false;
                }
                else
                {
                    retval.Append(charArray[pos]);
                }
            }
        }

        private bool TryOption(ref int pos)
        {
            int savedpos = pos;

            if (ParseValue(savedpos) == Token.OptionStarter2)
                savedpos++;

            if (ParseValue(savedpos) == Token.OptionStarter)
            {
                savedpos++;
                if (ParseValue(savedpos) == Token.OptionStarter)
                {
                    savedpos++;
                }
            }

            if (ParseValue(savedpos) == Token.AlphaNumeric)
            {
                StringBuilder sb = new StringBuilder(50);

                do
                {
                    sb.Append(charArray[savedpos]);
                    savedpos++;
                } while (ParseValue(savedpos) == Token.AlphaNumeric);

                if (ParseValue(savedpos) == Token.OptionDivider)
                {
                    savedpos++;

                    if(ParseValue(savedpos) == Token.Amper)
                    {
                        StringBuilder sb2=new StringBuilder(50);

                        if (ParseAmperString(sb2, ref savedpos) && sb2.Length > 0)
                        {
                            if (TestForOption(sb.ToString(), sb2.ToString()))
                            {
                                pos = savedpos;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        StringBuilder sb2=new StringBuilder(50);
                        
                        while (ParseValue(savedpos) != Token.WhiteSpace && ParseValue(savedpos) != Token.End)
                        {
                            sb2.Append(charArray[savedpos]);
                            savedpos++;
                        }

                        if(sb2.Length>0)
                        {
                            if(TestForOption(sb.ToString(),sb2.ToString()))
                            {
                                pos = savedpos;
                                return true;
                            }
                        
                        }
                    }
                }
                else if (ParseValue(savedpos) == Token.WhiteSpace || ParseValue(savedpos) == Token.End)
                {
                    var option = (from obj in currentCommand.Options
                                  where obj.OptionValueValidator == null ||
                                        obj.OptionValueValidator != null && obj.OptionValueValidator.ValueOptional
                                  from obj2 in obj.Token.SplitInternal()
                                  where sb.ToString() == obj2
                                  select obj).SingleOrDefault();
                        //MP: exist there an validation that this can only be single value


                    if (option != null)
                    {
                        if (!option.IsSet)
                        {
                            option.Set=true;
                            pos = savedpos;
                            return true;
                        }
                    }
                }

            }

            return false;
        }

        private bool TestForOption(string token,string value)
        {
            var options = from obj in currentCommand.Options
                         where obj.OptionValueValidator is StaticOptionValueValidator
                         from obj2 in obj.Token.SplitInternal()
                         where token == obj2
                         select obj;

            foreach (var option in options)
            {
                if (CanSetValueOption(option))
                if(option.OptionValueValidator.IsMatch(value))
                {
                    if(!option.IsSet)
                    {
                        option._Values.Add(value);
                        option.Set=true;
                        return true;
                    }
                    
                }
            }

            var options2 = from obj in currentCommand.Options
                          where obj.OptionValueValidator is RegularExpressionOptionValueValidator
                          from obj2 in obj.Token.SplitInternal()
                          where token == obj2
                          select obj;

            foreach (var option in options2)
            {
                if (CanSetValueOption(option))
                if (option.OptionValueValidator.IsMatch(value))
                {
                    if (option._Values.Count < option.OptionValueValidator.MaximumOccurrence)
                    {
                        option._Values.Add(value);
                        option.Set=true;
                        return true;
                    }

                }
            }

            return false;
        }

        private bool CanSetValueOption(Option option)
        {
            if (    (option.OptionValueValidator.ValueOptional && !option.IsSet) ||
                    (option.OptionValueValidator.ValueOptional && option.IsSet && option._Values.Count > 0) ||
                    (!option.OptionValueValidator.ValueOptional)
                )
                return true;

            return false;
        }

        private bool TryCommand(ref int pos)
        {
            int savedpos = pos;

            StringBuilder sb = new StringBuilder(50);

            do
            {
                sb.Append(charArray[savedpos]);
                savedpos++;
            } while (ParseValue(savedpos) == Token.AlphaNumeric);

            if (ParseValue(savedpos) == Token.WhiteSpace || ParseValue(savedpos) == Token.End)
            {
                Command command = (from obj in currentCommand.Commands
                                   from obj2 in obj.Token.SplitInternal()
                                   where sb.ToString() == obj2
                                   select obj).SingleOrDefault();

                if(command != null)
                {
                    command.Set=true;
                    currentCommand = command;
                    pos = savedpos;
                    return true;
                }
            }
            
            return false;
        }
    }
}
