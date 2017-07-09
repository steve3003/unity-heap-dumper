using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityHeapDump
{
    public interface IDumpContext
    {
        IFactory<ITypeData, Type> TypeDataFactory { get; }
        IFactory<IInstanceData, object> InstanceDataFactory { get; }
        IFactory<IFieldData, FieldInfo, object> FieldDataFactory { get; }
    }
}
