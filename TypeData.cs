using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace UnityHeapDumper
{
    public class TypeData : ITypeData
    {
        private Type type;
        private int size;
        private List<IFieldData> staticFields;
        private List<FieldInfo> instanceFields;
        private bool isPureValueType;

        public void Init(IDumpContext dumpContext, Type type)
        {
            this.type = type;

            size = 0;

            var typeDataFactory = dumpContext.TypeDataFactory;
            instanceFields = GetInstanceFields(typeDataFactory);

            isPureValueType = IsPureValueType(typeDataFactory);
            if (isPureValueType)
            {
                if (type.IsEnum)
                {
                    Type uderlyingType = Enum.GetUnderlyingType(type);
                    size = Marshal.SizeOf(uderlyingType);
                }
                else
                {
                    size = Marshal.SizeOf(type);
                }
            }

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
