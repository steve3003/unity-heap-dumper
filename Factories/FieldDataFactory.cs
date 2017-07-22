using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityHeapDumper
{
    public class FieldDataFactory : IFieldDataFactory
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

        IFieldData IFieldDataFactory.CreateArrayField(Array parent, int index)
        {
            return new ArrayFieldData(dumpContext, parent, index);
        }
    }
}

