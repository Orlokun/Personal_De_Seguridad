namespace GameManagement.Modules
{
    public interface IProfileModule
    {
        public int ElementsActive { get; }
        public bool IsModuleActive { get; }
    }
}