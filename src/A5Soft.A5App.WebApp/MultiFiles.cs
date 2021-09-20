using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A5Soft.A5App.Domain;

namespace A5Soft.A5App.WebApp
{
    public class MultiFiles : IMultiFiles
    {
        private List<FileItem> _files = new List<FileItem>();

        public string Name { get; set; }
        public List<FileItem> Files => _files;

        IList<IFileItem> IMultiFiles.Files => _files.ConvertAll(f => (IFileItem) f);
    }
}
