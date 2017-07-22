using System;
using System.Reflection;

namespace UnityHeapDumper
{
    public interface IFieldDataFactory : IFactory<IFieldData, FieldInfo, object>
    {
        IFieldData CreateArrayField(Array obj, int index);
    }
}

