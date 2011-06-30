using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MPOptions.Internal;

namespace NewStyle
{

    //public interface IGeneralFlow<T> where T:Command
    //{
    //    T Add(params Option[] options);

    //    T Add(params Command[] commands);

    //    T Add(params Argument[] arguments);
    //}

    ////public class RootCommand: Command, IGeneralFlow<RootCommand>
    ////{
    ////    private RootCommand():base("","")
    ////    {
    ////    }

    ////    public static RootCommand Get()
    ////    {
    ////        return new RootCommand();
    ////    }

    ////    public new RootCommand Add(params Option[] options)
    ////    {
    ////        return this;
    ////    }

    ////    public new RootCommand Add(params Command[] commands)
    ////    {
    ////        return this;
    ////    }


    ////    public new RootCommand Add(params Argument[] arguments)
    ////    {
    ////        return this;
    ////    }


    ////    //public CommandCollection Commands
    ////    //{
    ////    //    get; set;
    ////    //}

    ////    //public OptionCollection Options
    ////    //{
    ////    //    get;
    ////    //    set;
    ////    //}

    ////    //public ArgumentCollection Arguments
    ////    //{
    ////    //    get;
    ////    //    set;
    ////    //}

    ////}

    //public class ElementCollection<T>: KeyedCollection<string,T> where T:Element
    //{

    //    protected override string GetKeyForItem(T item)
    //    {
    //        return item.Name;
    //    }
    //}

    //public class CommandCollection : ElementCollection<Command>
    //{
    //    protected override void InsertItem(int index, Command item)
    //    {
    //        Validate(item);
    //        base.InsertItem(index, item);
    //    }

    //    private const string TokenRegex = @"^((\s*\w+\s*)|(\s*\w+\s*;\s*)|(\s*\w+(\s*;\s*\w+\s*(;)?\s*)*))$";

    //    private void Validate(Command obj)
    //    {
    //         //Test that CommandValues meet the RegexRequirements
    //        if (!Regex.IsMatch(obj.Token, TokenRegex))
    //        {
    //            ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InValidForm, ExceptionArgument.token);
    //        }

    //        ////Test that no sibling has same Name
    //        //var name = from ii in obj.ParentCommand.Commands
    //        //           where ii.Name==obj.Name
    //        //           select ii;
    //        //if(name.Count()>0)
    //        //{
    //        //    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlreadyInDictionary, ExceptionArgument.name);
    //        //}

    //        //Test that no sibling has same Token
    //        var strings = from ii in this
    //                      from iii in ii.Token.SplitInternal()
    //                      from iiii in obj.Token.SplitInternal()
    //                      where iii == iiii
    //                      select iiii;
    //        if(strings.Count()>0)
    //        {
    //            ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_TokenPartAlreadyInDictionary, ExceptionArgument.token);
    //        }
    //    }
    //}

    //public class OptionCollection : ElementCollection<Option>
    //{

    //}

    //public class ArgumentCollection : ElementCollection<Argument>
    //{
    //    protected override void  InsertItem(int index, Argument item)
    //    {
    //        Validate(item);
    //        base.InsertItem(index, item);
    //    }


    //    private void Validate(Argument obj)
    //    {
    //        //dont need to check the "name" for duplicity because it throws exception when it tries to add argument to argumentcollection

    //        //MP: for now only allow one argument per command
    //        //if (obj.ParentCommand.Arguments.Count() > 0)
    //        //{
    //        //    if (obj.ParentCommand.Arguments.Count() > 1 || obj.ParentCommand.Arguments.First() != obj)
    //        //        ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);
    //        //}
    //    }
    //}



    //public class Command : Element, IGeneralFlow<Command>
    //{
    //    internal Command(string name, string token)
    //        : base(name)
    //    {
    //        this.Token = token;
    //    }

    //    public string Token
    //    {
    //        get;
    //        private set;
    //    }

    //    public CommandCollection Commands
    //    {
    //        get; set;
    //    }

    //    public OptionCollection Options
    //    {
    //        get;set;
    //    }

    //    public ArgumentCollection Arguments
    //    {
    //        get; set;
    //    }

    //    #region IGeneralFlow<Command> Members

    //    public Command Add(params Option[] options)
    //    {
    //        return this;
    //    }

    //    public Command Add(params Command[] commands)
    //    {
    //        return this;
    //    }

    //    public Command Add(params Argument[] arguments)
    //    {
    //        return this;
    //    }

    //    #endregion
    //}

    //public abstract class Element
    //{
    //    internal Element(string name)
    //    {
         
    //        this.Name = name;
    //    }

    //    public string Name
    //    {
    //        get;
    //        private set;
    //    }

    //}

    //public class Option : Element
    //{
    //    internal Option(string name, string token, bool globalOption)
    //        : base(name)
    //    {
    //        _GlobalOption = globalOption;
    //        this.Token = token;
    //    }

    //    public string Token
    //    {
    //        get;
    //        private set;
    //    }

    //    public IOptionValueValidator OptionValueValidator
    //    {
    //        set;
    //        get;
    //    }

    //    private readonly bool _GlobalOption = false;
    //    public bool IsGlobalOption
    //    {
    //        get
    //        {
    //            return _GlobalOption;
    //            //return ParentCommand == null; 
    //            //StateBag.Options.ContainsKey(":: " + Name);
    //        }
    //    }
       
    //}

    //public interface IOptionValueValidator
    //{
       
    //    bool IsMatch(string value);

    //    int MaximumOccurrence
    //    { get; }

    //    bool ValueOptional
    //    { get; }
    //}

    //public class Argument : Element
    //{
    //    internal Argument(string name, IArgumentValidator argumentValidator)
    //        : base(name)
    //    {
    //        ArgumentValidator = argumentValidator;
    //    }

    //    internal IArgumentValidator ArgumentValidator
    //    {
    //        private set;
    //        get;
    //    }
    //}

    //public interface IArgumentValidator
    //{
    //    bool IsMatch(string value);

    //    int MaximumOccurrence
    //    { get; }
    //}

}
