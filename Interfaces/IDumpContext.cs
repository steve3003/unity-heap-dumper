using System;

namespace UnityHeapDumper
{
    public interface IDumpContext
    {
        IFactory<ITypeData, Type> TypeDataFactory { get; }
        IFactory<IInstanceData, object> InstanceDataFactory { get; }
        IFieldDataFactory FieldDataFactory { get; }
    }
}
