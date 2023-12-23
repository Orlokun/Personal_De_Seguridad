using DataUnits.ItemScriptableObjects;
using Utils;

namespace Players_NPC
{
    public class BaseGuardGameObject : BaseCharacterInScene, IInitialize
    {
        private IGuardStats _myStats;
        public IGuardStats Stats => _myStats;
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }
    
        public bool IsInitialized { get; }
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}