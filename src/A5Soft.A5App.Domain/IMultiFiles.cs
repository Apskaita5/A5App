using System;
using System.Collections.Generic;
using System.Text;

namespace A5Soft.A5App.Domain
{
    public interface IMultiFiles
    {
        string Name { get; }

        IList<IFileItem> Files { get; }
    }
}
