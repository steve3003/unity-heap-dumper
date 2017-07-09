using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityHeapDump
{
    public interface IHeapDump
    {
        void Dump(string path);
    }
}
