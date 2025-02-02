using System.Linq;
using UI.PopUpManager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum HighlightObjectID
    {
        Tablet,
        TabletVTab,
        TabletHTab,
        Clock, 
        BaseInfo,
        GamePlayVertical,
    }
    
    public class TutorialMaskOperator : PopUpObject, ITutorialMaskOperator
    {
        
        [SerializeField] private Sprite mRoundSprite;
        [SerializeField] private Sprite mRectSprite;
        
        [SerializeField]private Image mFeedbackMask;
        [SerializeField] private GameObject mDialogueMaskObject;
        
        private string[] mLastHighlight;
        
        private void Awake()
        {
            ToggleMaskActive(false);
        }

        public string[] GetLastHighlight => mLastHighlight;

        public void SetHighlightState(string[] newHightlight)
        {
            mLastHighlight = null;
            mLastHighlight = newHightlight;
            ProcessHighlightEvent();
        }

        private void ProcessHighlightEvent()
        {
            switch (mLastHighlight[0])
            {
                case "Tablet":
                    SetMaskType(false);
                    ProcessHighlightedObject(HighlightObjectID.Tablet);
                    break;                
                case "Tablet_VTab":
                    SetMaskType(false);
                    ProcessHighlightedObject(HighlightObjectID.TabletVTab);
                    break;
                case "Tablet_HTab":
                    SetMaskType(false);
                    ProcessHighlightedObject(HighlightObjectID.TabletHTab);
                    break;
                case "Clock":
                    SetMaskType(false);
                    ProcessHighlightedObject(HighlightObjectID.Clock);
                    break;
                case "Resources":
                    SetMaskType(false);
                    ProcessHighlightedObject(HighlightObjectID.BaseInfo);
                    break;
            }
        }

        private void ProcessHighlightedObject(HighlightObjectID highlightId)
        {
            var highlightObjects = FindObjectsByType<UIHighlightComponent>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var isAvailable = highlightObjects.Any(x=> x.HighlightObjectID == highlightId);
            if (isAvailable)
            {
                SetMaskSizeAndPosition(highlightObjects.First(x => x.HighlightObjectID == highlightId).GetRectTransform);
                ToggleMaskActive(true);
            }
        }

        private void SetMaskType(bool isRound)
        {
            mFeedbackMask.sprite = isRound ? mRoundSprite : mRectSprite;
        }
        public void SetMaskSizeAndPosition(RectTransform rectTransform)
        {
            mFeedbackMask.rectTransform.anchorMin = rectTransform.anchorMin;
            mFeedbackMask.rectTransform.anchorMax = rectTransform.anchorMax;
            
            mFeedbackMask.rectTransform.offsetMax = rectTransform.offsetMax;
            mFeedbackMask.rectTransform.offsetMin = rectTransform.offsetMin;
        }

        public void ToggleMaskActive(bool isActive)
        {
            mFeedbackMask.gameObject.SetActive(isActive);
            mDialogueMaskObject.SetActive(isActive);
        }
    }

    public interface ITutorialMaskOperator
    {
        public string[] GetLastHighlight{get;}
        public void SetHighlightState(string[] newHightlight);
    }
}