using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.Extensions
{
    public static class GeneralFlowExtensions
    {
        //public static T Add<T>(this IGeneralFlow<T> element, IEnumerable<Option> object[] objs)
        //{
        //    return element.Add(from i in objs
        //                       where i is Command
        //}

        public static T Add<T>(this IGeneralFlow<T> element, params object[] objs) where T:Command
        {
            return (T)((T)element).Add(
                                (from i in objs
                                 where i is Command
                                 select i as Command).ToArray()
                               )
                               .Add(
                               (from i in objs
                                where i is Option
                                select i as Option).ToArray()
                               )
                               .Add
                               (
                               (from i in objs
                                where i is Argument
                                select i as Argument).SingleOrDefault()
                               );

        }

        //public static Command Add(this IGeneralFlow<Command> element, params object[] objs)
        //{
        //    return element.Add(
        //                        (from i in objs
        //                         where i is Command
        //                         select i as Command).ToArray()
        //                       )
        //                       .Add(
        //                       (from i in objs
        //                        where i is Option
        //                        select i as Option).ToArray()
        //                       )
        //                       .Add
        //                       (
        //                       (from i in objs
        //                        where i is Argument
        //                        select i as Argument).SingleOrDefault()
        //                       );

        //}

        //public static RootCommand Add(this IGeneralFlow<RootCommand> element, params object[] objs)
        //{
        //    return element.Add(
        //                        (from i in objs
        //                         where i is Command
        //                         select i as Command).ToArray())
        //                       .Add(
        //                       (from i in objs
        //                        where i is Option
        //                        select i as Option).ToArray())
        //                       .Add
        //                       ((from i in objs
        //                         where i is Argument
        //                         select i as Argument).SingleOrDefault());

        //}
    }
}
