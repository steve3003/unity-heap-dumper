using System.Collections.Generic;

namespace UnityHeapDumper
{
    public interface IInstanceData
    {
        object Object { get; }
        int Id { get; }
        ITypeData TypeData { get; }
        IList<IFieldData> Fields { get; }
        void Init(IDumpContext dumpContext, object obj, int id);
        int GetSize(ICollection<int> seenInstances = null);
    }
}
