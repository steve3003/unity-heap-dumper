using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityHeapDump
{
    public class TypeData : ITypeData
    {
        private Type type;
        private int size;
        private int staticSize;
        private List<IFieldData> staticFields;
        private List<FieldInfo> dynamicFields;

        public void Init(IDumpContext dumpContext, Type type)
        {
            this.type = type;

            size = 0;
            staticSize = 0;

            var typeDataFactory = dumpContext.TypeDataFactory;
            dynamicFields = GetDynamicFields(typeDataFactory);

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

        private List<FieldInfo> GetDynamicFields(IFactory<ITypeData, Type> typeDataFactory)
        {
            var dynamicFieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var dynamicFields = new List<FieldInfo>(dynamicFieldInfos.Length);
            dynamicFields.AddRange(dynamicFieldInfos);

            var baseType = type.BaseType;
            if (baseType != null)
            {
                var baseTypeData = typeDataFactory.Create(baseType);
                dynamicFields.AddRange(baseTypeData.DynamicFields);
            }

            return dynamicFields;
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

        IList<FieldInfo> ITypeData.DynamicFields
        {
            get
            {
                return dynamicFields;
            }
        }
    }
}
