using System;
using System.Collections.Generic;
using System.Text;


namespace MTV.Library.Core.Extensions
{
    public interface IExtension
    {
        string Name { get; }

        IUIExtension UIExtension { get; }

    }
}
