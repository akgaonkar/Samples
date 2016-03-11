using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputExecuter
{
    public class NetworkShareFilereader: IFileReader
    {
        public System.IO.FileStream ReadFile()
        {
            return File.Create("C:/TEST/FILE.doc");
        }
    }
}
