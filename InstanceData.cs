﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityHeapDumper
{
    public class InstanceData : IInstanceData
    {
        private static readonly IFieldData[] emptyFields = new IFieldData[0];

        private object obj;
        private int id;
        private int size;
        private ITypeData typeData;
        private IList<IFieldData> fields;

        void IInstanceData.Init(IDumpContext dumpContext, object obj, int id)
        {
            this.obj = obj;
            this.id = id;

            size = 0;
            var type = obj.GetType();
            var typeDataFactory = dumpContext.TypeDataFactory;
            typeData = typeDataFactory.Create(type);
            size += typeData.StaticSize;

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
