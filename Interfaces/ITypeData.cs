using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityHeapDumper
{
    public interface ITypeData
    {
        Type Type { get; }
        int Size { get; }
        IList<IFieldData> StaticFields { get; }
        IList<FieldInfo> InstanceFields { get; }
        bool IsPureValueType { get; }
    }
}
