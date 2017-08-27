using System.Collections.Generic;

namespace UnityHeapDumper
{
    public class NullInstanceData : IInstanceData
    {
        private static readonly IFieldData[] emptyFields = new IFieldData[0];

        int IInstanceData.Id
        {
            get
            {
                return -1;
            }
        }

        int IInstanceData.GetSize(ICollection<int> seenInstances)
        {
            return 0;
        }

        ITypeData IInstanceData.TypeData
        {
            get
            {
                return null;
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
                return null;
            }
        }

        void IInstanceData.Init(IDumpContext dumpContext, object obj, int id)
        {
        }
    }
}
