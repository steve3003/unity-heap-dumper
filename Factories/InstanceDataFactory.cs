using System;
using System.Collections.Generic;

namespace UnityHeapDumper
{
    public class InstanceDataFactory : IFactory<IInstanceData, object>
    {
        private Dictionary<object, IInstanceData> instances = new Dictionary<object, IInstanceData>();
        private IDumpContext dumpContext;
        private static readonly NullInstanceData nullInstanceData = new NullInstanceData();

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

            IInstanceData instanceData;
            if (instances.TryGetValue(obj, out instanceData))
            {
                return instanceData;
            }

            instanceData = CreateInstanceData(obj);
            instances.Add(obj, instanceData);
            instanceData.Init(dumpContext, obj, instances.Count);

            return instanceData;
        }

        private IInstanceData CreateInstanceData(object obj)
        {
            var type = obj.GetType();

            if (type == typeof(string))
            {
                return new StringInstanceData();
            }

            if (type.IsArray)
            {
                return new ArrayInstanceData();
            }

            return new InstanceData();
        }
    }
}

