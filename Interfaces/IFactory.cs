
namespace UnityHeapDump
{
    public interface IFactory<T>
    {
        T Create();
    }

    public interface IFactory<TType, TParam>
    {
        TType Create(TParam parameter);
    }

    public interface IFactory<TType, TParam1, TParam2>
    {
        TType Create(TParam1 parameter1, TParam2 parameter2);
    }
}

