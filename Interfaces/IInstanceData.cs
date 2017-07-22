using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityHeapDumper
{
    public interface IInstanceData
    {
        object Object { get; }
        int Id { get; }
        int Size { get; }
        ITypeData TypeData { get; }
        IList<IFieldData> Fields { get; }
        void Init(IDumpContext dumpContext, object obj, int id);
    }
}
