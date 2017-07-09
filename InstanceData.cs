using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityHeapDump
{
    public class InstanceData : IInstanceData
    {
        private object obj;
        private int id;
        private int size;
        private ITypeData typeData;
        private List<IFieldData> fields;

        public void Init(IDumpContext dumpContext, object obj, int id)
        {
            this.obj = obj;
            this.id = id;

            size = 0;
            var type = obj.GetType();
            var typeDataFactory = dumpContext.TypeDataFactory;
            typeData = typeDataFactory.Create(type);
            size += typeData.StaticSize;

            var fieldDataFactory = dumpContext.FieldDataFactory;
            var dynamicFields = typeData.DynamicFields;
            fields = new List<IFieldData>(dynamicFields.Count);
            foreach (var fieldInfo in dynamicFields)
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

        int IInstanceData.Size
        {
            get
            {
                return size;
            }
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
