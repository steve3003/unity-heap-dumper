using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            size = 3 * IntPtr.Size + 2;
            size += str.Length * sizeof(char);
            int pad = size % IntPtr.Size;
            if (pad != 0)
            {
                size += IntPtr.Size - pad;
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
