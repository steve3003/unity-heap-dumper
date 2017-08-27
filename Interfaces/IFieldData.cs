namespace UnityHeapDumper
{
    public interface IFieldData
    {
        string Name { get; }
        string DeclaringType { get; }
        IInstanceData InstanceData { get; }
    }
}
