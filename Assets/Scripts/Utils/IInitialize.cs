namespace Utils
{
    public interface IInitialize
    {
        public bool IsInitialized { get;}
        public void Initialize();
    }

    public interface IInitializeWithArg<T1>
    {
        public void Initialize(T1 injectionClass);
    }
}