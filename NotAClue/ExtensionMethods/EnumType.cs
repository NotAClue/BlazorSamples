using System;
using System.Collections.Generic;
//using Microsoft.SqlServer.Management.Smo;

namespace NotAClue
{
    public class EnumType
    {
        public EnumType()
        {
            Values = new Dictionary<string, string>();
        }

        public String Name { get; set; }
        public Dictionary<string, string> Values { get; set; }
    }
}
