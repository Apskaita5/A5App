using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace A5Soft.A5App.Domain
{
    public interface IFileItem
    {
        string Description { get; }

        Stream FileStream { get; }
    }
}
