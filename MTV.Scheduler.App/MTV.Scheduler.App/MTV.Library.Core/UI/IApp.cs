using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MTV.Library.Core.UI
{
    public interface IApp : IDisposable
    {

        Form MainForm { get; }   

        void Start(string[] args);

    }
}
