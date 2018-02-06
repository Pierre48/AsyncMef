using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using AsyncMefLauncher.ADllWithExport.Contract;
using Catalog;
using System.Linq;
using System.ComponentModel.Composition.Primitives;

namespace AsyncMefTest
{
    [TestClass]
    public class AsyncCatalogTest
    {
        [TestMethod]
        public void AssemblyCatalogTest()
        {
            RunTest(f => new AssemblyCatalog(f));
        }


        [TestMethod]
        public void AsyncAssemblyCatalogTest()
        {
            RunTest(f => new AsyncAssemblyCatalog(f));
        }

        private void RunTest(Func<string, ComposablePartCatalog> GetCatalog)
        {
            var aggregateCatalog = new AggregateCatalog();

            //Build the directory path where the parts will be available
            var directoryPath =
                string.Concat(Path.GetDirectoryName
        (Assembly.GetExecutingAssembly().Location)
                          .Split('\\').Reverse().Skip(3).Reverse().Aggregate
        ((a, b) => a + "\\" + b)
                            , "\\", "Assemblies");

            foreach (var file in Directory.GetFiles(directoryPath, "*.dll"))
            {
                var asmCatalog = GetCatalog(file);
                aggregateCatalog.Catalogs.Add(asmCatalog);
                Console.WriteLine($"{file} loaded");
            }
            //Crete the composition container
            var container = new CompositionContainer(aggregateCatalog);

            Console.WriteLine($" Composition container loaded");
            // Composable parts are created here i.e. 
            // the Import and Export components assembles here
            container.Compose(new CompositionBatch());
            Console.WriteLine($" Composed");

            container.GetExport<ITest>().Value.Run();
        }
    }
}
