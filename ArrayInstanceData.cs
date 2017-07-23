using System;
using System.Collections.Generic;

namespace UnityHeapDumper
{
    public class ArrayInstanceData : IInstanceData
    {
        private List<IFieldData> elements;
        private int id;
        private Array array;
        private ITypeData typeData;
        private int typeSize;
        private HashSet<int> seenInstances;

        void IInstanceData.Init(IDumpContext dumpContext, object obj, int id)
        {
            seenInstances = new HashSet<int>();

            array = (Array)obj;
            this.id = id;

            var typeDataFactory = dumpContext.TypeDataFactory;
            var type = obj.GetType();
            typeData = typeDataFactory.Create(type);
            typeSize = typeData.Size;

            var fieldDataFactory = dumpContext.FieldDataFactory;
            var arrayLength = array.Length;
            elements = new List<IFieldData>(arrayLength);
            for (int i = 0; i < arrayLength; ++i)
            {
                var fieldData = fieldDataFactory.CreateArrayField(array, i);
                elements.Add(fieldData);
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

            foreach (var field in elements)
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
                return elements;
            }
        }

        object IInstanceData.Object
        {
            get
            {
                return array;
            }
        }
    }
}