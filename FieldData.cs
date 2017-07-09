using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityHeapDump
{
    public class FieldData : IFieldData
    {
        private static NullInstanceData nullInstanceData = new NullInstanceData();

        private FieldInfo fieldInfo;
        private IInstanceData instanceData;

        public FieldData(IDumpContext dumpContext, FieldInfo fieldInfo, object parent)
        {
            this.fieldInfo = fieldInfo;
            instanceData = nullInstanceData;
            if (fieldInfo.FieldType.IsPointer)
            {
                return;
            }

            try
            {
                var value = fieldInfo.GetValue(parent);
                var instanceDataFactory = dumpContext.InstanceDataFactory;
                instanceData = instanceDataFactory.Create(value);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }

        FieldInfo IFieldData.FieldInfo
        {
            get
            {
                return fieldInfo;
            }
        }

        IInstanceData IFieldData.InstanceData
        {
            get
            {
                return instanceData;
            }
        }
    }
}
