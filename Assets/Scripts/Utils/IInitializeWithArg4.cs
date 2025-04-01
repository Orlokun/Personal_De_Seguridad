namespace Utils
{
    public interface IInitializeWithArg4<T1, T2,T3,T4> 
    {
        public void Initialize(T1 injectionClass1, T2 injectionClass2, T3 injectionClass3, T4 injectionClass4);
    }
}