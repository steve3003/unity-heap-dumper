using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityHeapDump
{
    public interface ITypeData
    {
        Type Type { get; }
        int Size { get; }
        int StaticSize { get; }
        IList<IFieldData> StaticFields { get; }
        IList<FieldInfo> DynamicFields { get; }
    }
}
