namespace GamePlayManagement.ComplianceSystem
{
    public interface IComplianceObject
    {
        public IComplianceObjectData GetComplianceObjectData { get; }
        public void MarkAsActive();
        public void MarkOneAction();
        public int ComplianceCurrentCount { get; }
        public bool IsToleranceLevelReached { get; }
    }
}