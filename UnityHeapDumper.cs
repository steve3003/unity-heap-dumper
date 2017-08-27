using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityHeapDumper
{
    public class UnityHeapDumper : IHeapDumper, IDumpContext
    {
        [MenuItem("Tools/Dump")]
        private static void Dump()
        {
            IHeapDumper dumper = new UnityHeapDumper();
            dumper.Dump(@"C:\Users\stefa\Downloads\heap.xml");
        }

        private IFactory<ITypeData, Type> typeDataFactory;
        private IFactory<IInstanceData, object> instanceDataFactory;
        private IFieldDataFactory fieldDataFactory;
        private IDumpWriter dumpWriter;

        public UnityHeapDumper()
        {
            typeDataFactory = new TypeDataFactory(this);
            instanceDataFactory = new InstanceDataFactory(this);
            fieldDataFactory = new FieldDataFactory(this);
            dumpWriter = new XMLDumpWriter();
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

        IFieldDataFactory IDumpContext.FieldDataFactory
        {
            get
            {
                return fieldDataFactory;
            }
        }

        void IHeapDumper.Dump(string path)
        {
            List<IFieldData> staticFields = GetStaticFields();
            HashSet<int> seenInstances = new HashSet<int>();

            dumpWriter.Open(path);
            foreach (var staticField in staticFields)
            {
                Debug.LogFormat("type={0} field={1} size={2}", staticField.DeclaringType, staticField.Name, staticField.InstanceData.GetSize());
                seenInstances.Clear();
                dumpWriter.WriteField(staticField, seenInstances);
            }
            dumpWriter.Close();
        }

        private List<IFieldData> GetStaticFields()
        {
            var thisNamespace = typeof(UnityHeapDumper).Namespace;
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
