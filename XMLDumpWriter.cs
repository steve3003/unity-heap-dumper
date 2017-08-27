using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;

namespace UnityHeapDumper
{
    public class XMLDumpWriter : IDumpWriter
    {
        private StringBuilder builder = new StringBuilder();
        private string path;
        private bool open = false;
        private IDumpWriter thisDumpWriter;

        public XMLDumpWriter()
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
            builder.Append("<heap>");
        }

        void IDumpWriter.Close()
        {
            if (!open)
            {
                return;
            }
            open = false;

            builder.Append("</heap>");
            File.WriteAllText(path, builder.ToString());
        }

        void IDumpWriter.WriteField(IFieldData fieldData, ICollection<int> seenInstances)
        {
            builder.Append("<field>");
            var declaringType = fieldData.DeclaringType;
            if (!string.IsNullOrEmpty(declaringType))
            {
                builder.AppendFormat("<declaring_type>{0}</declaring_type>", SecurityElement.Escape(declaringType));
            }
            builder.AppendFormat("<name>{0}</name>", SecurityElement.Escape(fieldData.Name));
            thisDumpWriter.WriteInstance(fieldData.InstanceData, seenInstances);
            builder.Append("</field>");
        }

        void IDumpWriter.WriteInstance(IInstanceData instanceData, ICollection<int> seenInstances)
        {
            builder.Append("<instance>");
            var id = instanceData.Id;
            builder.AppendFormat("<id>{0}</id>", id);
            builder.AppendFormat("<type>{0}</type>", instanceData.TypeData == null ? "null" : SecurityElement.Escape(instanceData.TypeData.Type.Name));
            builder.AppendFormat("<size>{0}</size>", instanceData.GetSize(seenInstances));
            var fields = instanceData.Fields;
            if (fields.Count > 0)
            {
                if (!seenInstances.Contains(id))
                {
                    seenInstances.Add(id);

                    builder.Append("<fields>");
                    foreach (var fieldData in fields)
                    {
                        thisDumpWriter.WriteField(fieldData, seenInstances);
                    }
                    builder.Append("</fields>");
                }
                else
                {
                    builder.Append("<recursion/>");
                }
            }
            builder.Append("</instance>");
        }
    }
}
