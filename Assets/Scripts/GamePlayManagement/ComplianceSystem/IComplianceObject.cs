namespace GamePlayManagement.ComplianceSystem
{
    public interface IComplianceObject
    {
        public IComplianceObjectData GetComplianceObjectData { get; }
        public void MarkAsActive();
    }
}