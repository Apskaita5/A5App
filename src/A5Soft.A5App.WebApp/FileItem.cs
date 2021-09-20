using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using A5Soft.A5App.Domain;

namespace A5Soft.A5App.WebApp
{
    public class FileItem : IFileItem
    {
        public string Description { get; set; }
        public Stream FileStream { get; set; }

    }
}
