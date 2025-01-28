using DialogueSystem;
using DialogueSystem.Interfaces;
using GameDirection;
using UnityEngine;
using Utils;

namespace GamePlayManagement.TutorialManagement
{
    public class TutorialGameManager : MonoBehaviour, IInitializeWithArg1<IGameDirector>
    {
        private IGameDirector _mGameDirector;
        private IBaseTutorialDialogueData _mTutorialDialogueData;

        private IDialogueOperator _mDialogueOperator;
        
        public bool IsInitialized => _mInitialized;
        private bool _mInitialized;

        private void Start()
        {
            var gDirector = GameDirector.Instance;
            Initialize(gDirector);
        }

        public void Initialize(IGameDirector injectionClass)
        {
            if (_mInitialized)
            {
                return;
            }
            _mDialogueOperator = DialogueOperator.Instance;
            _mGameDirector = injectionClass;
            
            _mInitialized = true;
            //Subscribe to dialogue node event?
        }
    }
    
    public enum FeedbackObjects
    {
        TABLET_VTAB = 1,
        TutorialPopUp = 2,
        TutorialUI = 4
    }
}