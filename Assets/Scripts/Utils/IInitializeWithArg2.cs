namespace Utils
{
    public interface IInitializeWithArg2<T1, T2> 
    {
        public void Initialize(T1 injectionClass1, T2 injectionClass2);
    }
}