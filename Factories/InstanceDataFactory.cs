using System;
using System.Collections.Generic;

namespace UnityHeapDump
{
    public class InstanceDataFactory : IFactory<IInstanceData, object>
    {
        private Dictionary<object, InstanceData> instances = new Dictionary<object, InstanceData>();
        private IDumpContext dumpContext;
        private static NullInstanceData nullInstanceData = new NullInstanceData();

        public InstanceDataFactory(IDumpContext dumpContext)
        {
            this.dumpContext = dumpContext;
        }

        IInstanceData IFactory<IInstanceData, object>.Create(object obj)
        {
            if (obj == null)
            {
                return nullInstanceData;
            }

            InstanceData instanceData;
            if (instances.TryGetValue(obj, out instanceData))
            {
                return instanceData;
            }

            instanceData = new InstanceData();
            instances.Add(obj, instanceData);
            instanceData.Init(dumpContext, obj, instances.Count);

            return instanceData;
        }
    }
}

