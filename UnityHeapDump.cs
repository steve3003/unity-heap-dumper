using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityHeapDump
{
    public class UnityHeapDump : IHeapDump, IDumpContext
    {
        [MenuItem("Tools/Dump")]
        private static void Dump()
        {
            IHeapDump dumper = new UnityHeapDump();
            dumper.Dump(@"C:\Users\stefa\Downloads\heap.xml");
        }

        private IFactory<ITypeData, Type> typeDataFactory;
        private IFactory<IInstanceData, object> instanceDataFactory;
        private IFactory<IFieldData, FieldInfo, object> fieldDataFactory;

        public UnityHeapDump()
        {
            typeDataFactory = new TypeDataFactory(this);
            instanceDataFactory = new InstanceDataFactory(this);
            fieldDataFactory = new FieldDataFactory(this);
        }

        IFactory<ITypeData, Type> IDumpContext.TypeDataFactory
        {
            get
            {
                return typeDataFactory;
            }
        }

        IFactory<IInstanceData, object> IDumpContext.InstanceDataFactory
        {
            get
            {
                return instanceDataFactory;
            }
        }

        IFactory<IFieldData, FieldInfo, object> IDumpContext.FieldDataFactory
        {
            get
            {
                return fieldDataFactory;
            }
        }

        void IHeapDump.Dump(string path)
        {
            List<IFieldData> staticFields = GetStaticFields();

            var builder = new StringBuilder();
            builder.Append("<heap>");
            HashSet<int> seenInstances = new HashSet<int>();
            foreach (var staticField in staticFields)
            {
                var fieldInfo = staticField.FieldInfo;
                Debug.LogFormat("type={0} field={1} size={2}", fieldInfo.DeclaringType.Name, fieldInfo.Name, staticField.InstanceData.Size);
                seenInstances.Clear();
                PrintField(builder, staticField, seenInstances);
            }
            builder.Append("</heap>");
            File.WriteAllText(path, builder.ToString());
        }

        private void PrintField(StringBuilder builder, IFieldData fieldData, HashSet<int> seenInstances)
        {
            var fieldInfo = fieldData.FieldInfo;
            builder.Append("<field>");
            builder.AppendFormat("<declaring_type>{0}</declaring_type>", SecurityElement.Escape(fieldInfo.DeclaringType.Name));
            builder.AppendFormat("<name>{0}</name>", SecurityElement.Escape(fieldInfo.Name));
            PrintInstance(builder, fieldData.InstanceData, seenInstances);
            builder.Append("</field>");
        }

        private void PrintInstance(StringBuilder builder, IInstanceData instanceData, HashSet<int> seenInstances)
        {
            builder.Append("<instance>");
            var id = instanceData.Id;
            builder.AppendFormat("<id>{0}</id>", id);
            builder.AppendFormat("<type>{0}</type>", instanceData.TypeData == null ? "null" : SecurityElement.Escape(instanceData.TypeData.Type.Name));
            builder.Append("<fields>");
            if (!seenInstances.Contains(id))
            {
                seenInstances.Add(id);
                foreach (var fieldData in instanceData.Fields)
                {
                    PrintField(builder, fieldData, seenInstances);
                }
            }
            else
            {
                builder.Append("<recursion/>");
            }
            builder.Append("</fields>");
            builder.Append("</instance>");
        }

        private List<IFieldData> GetStaticFields()
        {
            var thisNamespace = typeof(UnityHeapDump).Namespace;
            var staticFields = new List<IFieldData>();
            var assemblies = GetAppAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.Namespace == thisNamespace)
                    {
                        continue;
                    }

                    var typeData = typeDataFactory.Create(type);
                    IList<IFieldData> typeStaticFields = typeData.StaticFields;
                    if (typeStaticFields.Count > 0)
                    {
                        staticFields.AddRange(typeStaticFields);
                    }
                }
            }

            return staticFields;
        }

        private List<Assembly> GetAppAssemblies()
        {
            var appAssemblies = new List<Assembly>(3);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.Contains("Assembly-CSharp"))
                {
                    appAssemblies.Add(assembly);
                }
            }
            return appAssemblies;
        }
    }
}
