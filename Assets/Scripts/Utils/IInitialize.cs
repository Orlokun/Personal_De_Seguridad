namespace Utils
{
    public interface IInitialize
    {
        bool IsInitialized { get;}
        public void Initialize();
    }

    public interface IInitializeWithArg1<T1> 
    {
        public void Initialize(T1 injectionClass);
    }
    public interface IInitializeWithArg2<T1, T2> 
    {
        public void Initialize(T1 injectionClass1, T2 injectionClass2);
    }
}