using System;
using System.Collections.Generic;

namespace UnityHeapDump
{
    public class TypeDataFactory : IFactory<ITypeData, Type>
    {
        private Dictionary<Type, TypeData> types = new Dictionary<Type, TypeData>();
        private IDumpContext dumpContext;

        public TypeDataFactory(IDumpContext dumpContext)
        {
            this.dumpContext = dumpContext;
        }

        ITypeData IFactory<ITypeData, Type>.Create(Type type)
        {
            TypeData typeData;
            if (types.TryGetValue(type, out typeData))
            {
                return typeData;
            }

            typeData = new TypeData();
            types.Add(type, typeData);
            typeData.Init(dumpContext, type);

            return typeData;
        }
    }
}

