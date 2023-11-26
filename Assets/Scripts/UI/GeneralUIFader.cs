using UnityEngine;

namespace UI
{
    public class GeneralUIFader : MonoBehaviour, IGeneralUIFader
    {
        private static readonly int CurtainDisappear = Animator.StringToHash("ObjectFadeOut");
        private static readonly int CurtainAppear = Animator.StringToHash("ObjectFadeIn");
        [SerializeField] private Animator generalFadeOutPanelAnim;
        private void Start()
        {
            DontDestroyOnLoad(generalFadeOutPanelAnim.gameObject.transform.parent);
        }
        public void GeneralCurtainAppear()
        {
            Debug.Log("[GeneralCurtainIn] FADE IN CURTAIN");
            generalFadeOutPanelAnim.Play(CurtainAppear);
        }
        public void GeneralCurtainDisappear()
        {   
            Debug.Log("[GeneralCurtainOUT] FADE OUT CURTAIN");
            generalFadeOutPanelAnim.Play(CurtainDisappear);
        }
    }
}