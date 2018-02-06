using ADllWithExport.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                //Creating an instance of aggregate catalog. It aggregates other catalogs
                var aggregateCatalog = new AggregateCatalog();

                //Build the directory path where the parts will be available
                var directoryPath =
                    string.Concat(Path.GetDirectoryName
            (Assembly.GetExecutingAssembly().Location)
                              .Split('\\').Reverse().Skip(3).Reverse().Aggregate
            ((a, b) => a + "\\" + b)
                                , "\\", "Assemblies");

                //Load parts from the available DLLs in the specified path 
                //using the directory catalog
                var directoryCatalog = new DirectoryCatalog(directoryPath, "*.dll");
                Console.WriteLine($"{watch.ElapsedMilliseconds}\t directory catalog loaded");
                //Load parts from the current assembly if available
                var asmCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
                Console.WriteLine($"{watch.ElapsedMilliseconds}\t Assembly catalog loaded");

                //Add to the aggregate catalog
                aggregateCatalog.Catalogs.Add(directoryCatalog);
                aggregateCatalog.Catalogs.Add(asmCatalog);

                //Crete the composition container
                var container = new CompositionContainer(aggregateCatalog);

                Console.WriteLine($"{watch.ElapsedMilliseconds}\t Composition container loaded");
                // Composable parts are created here i.e. 
                // the Import and Export components assembles here
                container.Compose(new CompositionBatch());
                Console.WriteLine($"{watch.ElapsedMilliseconds}\t Composed");

                container.GetExport<ITest>().Value.Run();
                Console.WriteLine($"{watch.ElapsedMilliseconds}\t Run");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
