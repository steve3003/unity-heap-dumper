using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;

namespace UnityHeapDumper
{
    public class JSONDumpWriter : IDumpWriter
    {
        private StringBuilder builder = new StringBuilder();
        private string path;
        private bool open = false;
        private IDumpWriter thisDumpWriter;
        private bool firstElement = false;

        public JSONDumpWriter()
        {
            thisDumpWriter = this;
        }

        void IDumpWriter.Open(string path)
        {
            if (open)
            {
                return;
            }
            open = true;

            this.path = path;
            builder.Length = 0;
            builder.Append("{\"heap\":[");
            firstElement = true;
        }

        void IDumpWriter.Close()
        {
            if (!open)
            {
                return;
            }
            open = false;

            builder.Append("]}");
            File.WriteAllText(path, builder.ToString());
        }

        private void CheckIfFirstElement()
        {
            if (!firstElement)
            {
                builder.Append(",");
            }
            else
            {
                firstElement = false;
            }
        }

        void IDumpWriter.WriteField(IFieldData fieldData, ICollection<int> seenInstances)
        {
            CheckIfFirstElement();
            builder.Append("{\"field\":{");
            var declaringType = fieldData.DeclaringType;
            if (!string.IsNullOrEmpty(declaringType))
            {
                builder.AppendFormat("\"declaring_type\":\"{0}\",", SecurityElement.Escape(declaringType));
            }
            builder.AppendFormat("\"name\":\"{0}\"", SecurityElement.Escape(fieldData.Name));
            thisDumpWriter.WriteInstance(fieldData.InstanceData, seenInstances);
            builder.Append("}}");
        }

        void IDumpWriter.WriteInstance(IInstanceData instanceData, ICollection<int> seenInstances)
        {
            CheckIfFirstElement();
            builder.Append("\"instance\":{");
            var id = instanceData.Id;
            builder.AppendFormat("\"id\":{0}", id);
            builder.AppendFormat(",\"type\":\"{0}\"", instanceData.TypeData == null ? "null" : SecurityElement.Escape(instanceData.TypeData.Type.Name));
            builder.AppendFormat(",\"size\":{0}", instanceData.GetSize(seenInstances));
            var fields = instanceData.Fields;
            if (fields.Count > 0)
            {
                if (!seenInstances.Contains(id))
                {
                    seenInstances.Add(id);

                    builder.Append(",\"fields\":[");
                    firstElement = true;
                    foreach (var fieldData in fields)
                    {
                        thisDumpWriter.WriteField(fieldData, seenInstances);
                    }
                    builder.Append("]");
                }
                else
                {
                    builder.Append(",\"recursion\":{}");
                }
            }
            builder.Append("}");
        }
    }
}
