using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityHeapDump
{
    public class NullInstanceData : IInstanceData
    {
        private IFieldData[] emptyFileds = new IFieldData[0];

        int IInstanceData.Id
        {
            get
            {
                return -1;
            }
        }

        int IInstanceData.Size
        {
            get
            {
                return 0;
            }
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
                return emptyFileds;
            }
        }

        object IInstanceData.Object
        {
            get
            {
                return null;
            }
        }
    }
}
