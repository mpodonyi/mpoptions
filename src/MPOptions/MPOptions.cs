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


namespace MPOptions
{
    public static class MPOptions
    {
        public static RootCommand GetRoot()
        {
            return new RootCommand();
        }
    }
    
}