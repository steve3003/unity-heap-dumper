using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityHeapDump
{
    public interface IFieldData
    {
        FieldInfo FieldInfo { get; }
        IInstanceData InstanceData { get; }
    }
}
