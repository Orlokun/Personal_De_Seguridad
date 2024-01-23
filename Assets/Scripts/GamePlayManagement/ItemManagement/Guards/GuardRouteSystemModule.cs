namespace GamePlayManagement.ItemManagement.Guards
{
    public class GuardRouteSystemModule : IGuardRouteSystemModule
    {
        private IShopInspectionPosition _mInitialSpot;
        private IShopInspectionPosition _currentInspectionSpot;
        private bool _mIsInitialized;
        public bool IsInitialized => _mIsInitialized;

        public void Initialize(IShopInspectionPosition injectionClass)
        {
            if (_mIsInitialized)
            {
                return;
            }
            _mInitialSpot = injectionClass;
            _currentInspectionSpot = _mInitialSpot;
            _mIsInitialized = true;
        }

        public IShopInspectionPosition GetStartPosition => _mInitialSpot;
        public IShopInspectionPosition GetCurrentPosition => _currentInspectionSpot;
        public void SetNewCurrentPosition(IShopInspectionPosition newPosition)
        {
            _currentInspectionSpot = newPosition;
        }

    }
}