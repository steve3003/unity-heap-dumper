using System.Collections.Generic;

namespace UnityHeapDumper
{
    public class StringInstanceData : IInstanceData
    {
        private static readonly IFieldData[] emptyFields = new IFieldData[0];
        private int id;
        private string str;
        private ITypeData typeData;
        private int size;

        void IInstanceData.Init(IDumpContext dumpContext, object obj, int id)
        {
            str = (string)obj;
            this.id = id;

            var typeDataFactory = dumpContext.TypeDataFactory;
            typeData = typeDataFactory.Create(typeof(string));

            size = str.Length * sizeof(char);
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
                return emptyFields;
            }
        }

        object IInstanceData.Object
        {
            get
            {
                return str;
            }
        }
    }
}
