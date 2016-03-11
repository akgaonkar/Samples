using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class WordDocumentParser : IParser
    {
        public List<string> Parser()
        {
            //parsing logic
            return new List<string>() { "C#", "ASP.NET", "MVC", "WCF" };
        }
    }
}
