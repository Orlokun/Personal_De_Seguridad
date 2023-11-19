using System;
using UnityEngine;

namespace UI
{
    public class GeneralUIFader : MonoBehaviour, IGeneralUIFader
    {
        private static readonly int FadeIn = Animator.StringToHash("ObjectFadeOut");
        private static readonly int FadeOut = Animator.StringToHash("ObjectFadeIn");
        [SerializeField] private Animator GeneralFadeOutPanelAnim;
        private void Start()
        {
            DontDestroyOnLoad(GeneralFadeOutPanelAnim.gameObject.transform.parent);
        }
        public void GeneralFadeIn()
        {
            GeneralFadeOutPanelAnim.Play(FadeIn);
        }
        public void GeneralCameraFadeOut()
        {   
            GeneralFadeOutPanelAnim.Play(FadeOut);
        }
    }
}