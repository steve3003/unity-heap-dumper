using System;

namespace UnityHeapDumper
{
    public class ArrayFieldData : IFieldData
    {
        private static NullInstanceData nullInstanceData = new NullInstanceData();

        private IInstanceData instanceData;
        private int index;

        public ArrayFieldData(IDumpContext dumpContext, Array parent, int index)
        {
            this.index = index;
            instanceData = nullInstanceData;

            var value = parent.GetValue(index);
            var instanceDataFactory = dumpContext.InstanceDataFactory;
            instanceData = instanceDataFactory.Create(value);
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
                return index.ToString();
            }
        }

        string IFieldData.DeclaringType
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
