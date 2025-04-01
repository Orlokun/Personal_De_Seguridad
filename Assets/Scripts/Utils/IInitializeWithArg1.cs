namespace Utils
{
    public interface IInitializeWithArg1<T1> 
    {
        bool IsInitialized { get;}
        public void Initialize(T1 injectionClass);
    }
}