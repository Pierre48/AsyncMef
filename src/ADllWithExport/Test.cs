using ADllWithExport.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADllWithExport
{
    [Export(typeof(ITest))]
    class Test : ITest
    {
        public void Run() { Console.WriteLine("This is a test"); }
    }
}
