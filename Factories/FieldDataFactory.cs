using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityHeapDump
{
    public class FieldDataFactory : IFactory<IFieldData, FieldInfo, object>
    {
        private IDumpContext dumpContext;

        public FieldDataFactory(IDumpContext dumpContext)
        {
            this.dumpContext = dumpContext;
        }

        IFieldData IFactory<IFieldData, FieldInfo, object>.Create(FieldInfo fieldInfo, object parent)
        {
            return new FieldData(dumpContext, fieldInfo, parent);
        }
    }
}

