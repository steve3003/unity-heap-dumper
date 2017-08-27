using System.Collections.Generic;

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
