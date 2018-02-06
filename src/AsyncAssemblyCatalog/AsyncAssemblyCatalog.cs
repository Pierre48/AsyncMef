using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;

namespace Catalog
{
    public class AsyncAssemblyCatalog : ComposablePartCatalog, ICompositionElement
    {
        private readonly string _file;
        AssemblyCatalog _internalCatalog = null;
        private readonly string _fileName;

        public AsyncAssemblyCatalog(string file)
        {
            _file = file;
            _fileName = new FileInfo(file).Name.Replace(".dll", "");
        }

        public string DisplayName =>  "null";

        public ICompositionElement Origin =>  null;

        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {

            if (definition.ContractName.StartsWith(_fileName,StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Loaded");
                lock (this)
                {
                    if (_internalCatalog == null)
                        _internalCatalog = new AssemblyCatalog(_file);
                }
                try
                {
                    return _internalCatalog.GetExports(definition);
                }
                catch 
                {
                    return base.GetExports(definition); 
                }
            }
            return base.GetExports(definition);
        }
    }
}