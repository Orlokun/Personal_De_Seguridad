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
    public interface IInitializeWithArg4<T1, T2,T3,T4> 
    {
        public void Initialize(T1 injectionClass1, T2 injectionClass2, T3 injectionClass3, T4 injectionClass4);
    }
}