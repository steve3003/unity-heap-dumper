using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityHeapDumper
{
    public interface IDumpWriter
    {
        void Open(string path);
        void Close();
        void WriteField(IFieldData fieldData, ICollection<int> seenInstances);
        void WriteInstance(IInstanceData instanceData, ICollection<int> seenInstances);
    }
}
