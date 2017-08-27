using System;
using System.Reflection;

namespace UnityHeapDumper
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

        IInstanceData IFieldData.InstanceData
        {
            get
            {
                return instanceData;
            }
        }

        string IFieldData.Name
        {
            get
            {
                return fieldInfo.Name;
            }
        }

        string IFieldData.DeclaringType
        {
            get
            {
                return fieldInfo.DeclaringType.Name;
            }
        }
    }
}
