namespace Players_NPC
{
    public class StoreOwner : BaseCharacterInScene
    {
        private void Awake()
        {
            base.Awake();
            BaseAnimator.ChangeAnimationState("Sit");
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }
    }
}