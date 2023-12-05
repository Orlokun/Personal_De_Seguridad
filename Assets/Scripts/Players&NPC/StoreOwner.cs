namespace Players_NPC
{
    public class StoreOwner : BaseCharacterInScene
    {
        protected override void Awake()
        {
            base.Awake();
            BaseAnimator.ChangeAnimationState("Sit");
        }
    }
}