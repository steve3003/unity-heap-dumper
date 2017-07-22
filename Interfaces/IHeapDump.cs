using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityHeapDumper
{
    public interface IHeapDumper
    {
        void Dump(string path);
    }
}
