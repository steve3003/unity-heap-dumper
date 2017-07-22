using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityHeapDumper
{
    public interface IFieldData
    {
        string Name { get; }
        string DeclaringType { get; }
        IInstanceData InstanceData { get; }
    }
}
