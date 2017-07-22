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
        private int size;

        void IInstanceData.Init(IDumpContext dumpContext, object obj, int id)
        {
            array = (Array)obj;
            this.id = id;

            var typeDataFactory = dumpContext.TypeDataFactory;
            var type = obj.GetType();
            typeData = typeDataFactory.Create(type);

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