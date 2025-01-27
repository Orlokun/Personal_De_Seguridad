using GameDirection;
using UI.PopUpManager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FeedbackMaskObjects : PopUpObject
    {
        [SerializeField] private Sprite mRoundSprite;
        [SerializeField] private Sprite mRectSprite;
        
        [SerializeField]private Image mFeedbackMask;

        private void Awake()
        {
           
        }
    }
}