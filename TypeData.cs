using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityHeapDumper
{
    public class TypeData : ITypeData
    {
        private Type type;
        private int size;
        private int staticSize;
        private List<IFieldData> staticFields;
        private List<FieldInfo> instanceFields;
        private bool isPureValueType;

        public void Init(IDumpContext dumpContext, Type type)
        {
            this.type = type;

            size = 0;
            staticSize = 0;

            var typeDataFactory = dumpContext.TypeDataFactory;
            instanceFields = GetInstanceFields(typeDataFactory);

            isPureValueType = IsPureValueType(typeDataFactory);

            var fieldDataFactory = dumpContext.FieldDataFactory;
            staticFields = GetStaticFields(fieldDataFactory);
        }

        private List<IFieldData> GetStaticFields(IFactory<IFieldData, FieldInfo, object> fieldDataFactory)
        {
            var staticFieldInfos = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var staticFields = new List<IFieldData>(staticFieldInfos.Length);
            foreach (var fieldInfo in staticFieldInfos)
            {
                var fieldData = fieldDataFactory.Create(fieldInfo, null);
                staticFields.Add(fieldData);
            }
            return staticFields;
        }

        private List<FieldInfo> GetInstanceFields(IFactory<ITypeData, Type> typeDataFactory)
        {
            var instanceFieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var instanceFields = new List<FieldInfo>(instanceFieldInfos.Length);
            instanceFields.AddRange(instanceFieldInfos);

            var baseType = type.BaseType;
            if (baseType != null)
            {
                var baseTypeData = typeDataFactory.Create(baseType);
                instanceFields.AddRange(baseTypeData.InstanceFields);
            }

            return instanceFields;
        }

        private bool IsPureValueType(IFactory<ITypeData, Type> typeDataFactory)
        {
            if (!type.IsValueType)
            {
                return false;
            }

            if (type.IsPrimitive)
            {
                return true;
            }

            foreach (var instanceField in instanceFields)
            {
                var fieldType = instanceField.FieldType;
                var fieldTypeData = typeDataFactory.Create(fieldType);
                if (!fieldTypeData.IsPureValueType)
                {
                    return false;
                }
            }

            return true;
        }

        Type ITypeData.Type
        {
            get
            {
                return type;
            }
        }

        int ITypeData.Size
        {
            get
            {
                return size;
            }
        }

        int ITypeData.StaticSize
        {
            get
            {
                return staticSize;
            }
        }

        IList<IFieldData> ITypeData.StaticFields
        {
            get
            {
                return staticFields;
            }
        }

        IList<FieldInfo> ITypeData.InstanceFields
        {
            get
            {
                return instanceFields;
            }
        }

        bool ITypeData.IsPureValueType
        {
            get
            {
                return isPureValueType;
            }
        }
    }
}
