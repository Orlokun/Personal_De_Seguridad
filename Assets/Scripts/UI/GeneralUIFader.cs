using System;
using UnityEngine;

namespace UI
{
    public class GeneralUIFader : MonoBehaviour, IGeneralUIFader
    {
        private static readonly int FadeIn = Animator.StringToHash("FadeIn");
        [SerializeField] private Animator GeneralFadeOutPanelAnim;
        private void Start()
        {
            DontDestroyOnLoad(GeneralFadeOutPanelAnim.gameObject.transform.parent);
        }
        public void GeneralCameraFadeIn()
        {
            GeneralFadeOutPanelAnim.SetBool(FadeIn, true);
        }
        public void GeneralCameraFadeOut()
        {   
            GeneralFadeOutPanelAnim.SetBool(FadeIn, false);
        }
    }
}