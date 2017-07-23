using System.Collections.Generic;

namespace UnityHeapDumper
{
    public class InstanceData : IInstanceData
    {
        private static readonly IFieldData[] emptyFields = new IFieldData[0];

        private object obj;
        private int id;
        private int typeSize;
        private ITypeData typeData;
        private IList<IFieldData> fields;
        private HashSet<int> seenInstances;

        void IInstanceData.Init(IDumpContext dumpContext, object obj, int id)
        {
            seenInstances = new HashSet<int>();

            this.obj = obj;
            this.id = id;

            var type = obj.GetType();
            var typeDataFactory = dumpContext.TypeDataFactory;
            typeData = typeDataFactory.Create(type);
            typeSize = typeData.Size;

            if (typeData.IsPureValueType)
            {
                fields = emptyFields;
                return;
            }

            var fieldDataFactory = dumpContext.FieldDataFactory;
            var instanceFields = typeData.InstanceFields;
            fields = new List<IFieldData>(instanceFields.Count);
            foreach (var fieldInfo in instanceFields)
            {
                var fieldData = fieldDataFactory.Create(fieldInfo, obj);
                fields.Add(fieldData);
            }
        }

        int IInstanceData.Id
        {
            get
            {
                return id;
            }
        }

        int IInstanceData.GetSize(ICollection<int> seenInstances)
        {
            this.seenInstances.Clear();
            if (seenInstances != null)
            {
                if (seenInstances.Contains(id))
                {
                    return 0;
                }

                this.seenInstances.UnionWith(seenInstances);
            }
            this.seenInstances.Add(id);

            var size = typeSize;

            foreach (var field in fields)
            {
                var fieldInstanceData = field.InstanceData;
                size += fieldInstanceData.GetSize(this.seenInstances);
            }

            return size;
        }

        ITypeData IInstanceData.TypeData
        {
            get
            {
                return typeData;
            }
        }

        IList<IFieldData> IInstanceData.Fields
        {
            get
            {
                return fields;
            }
        }

        object IInstanceData.Object
        {
            get
            {
                return obj;
            }
        }
    }
}
