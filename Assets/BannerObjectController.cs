using System;
using System.Collections;
using GameDirection;
using GameDirection.TimeOfDayManagement;
using TMPro;
using UI.PopUpManager;
using UnityEngine;
using Utils;

public interface IBannerObjectController
{
    public void ToggleBannerForSeconds(string newText, float seconds);
}

public class CigarConfirmationPopUp : ConfirmationPopUp
{
    private void Start()
    {
        GameDirector.Instance.GetClockInDayManagement.PlayPauseClock(false);
    }

    public override void ConfirmAction()
    {
        GameDirector.Instance.GetClockInDayManagement.
    }
    public override void CancelAction()
    {
        
    }
}

public interface IConfirmationPopUp
{
    void CancelAction();
    void ConfirmAction();
}

public class ConfirmationPopUp : PopUpObject, IConfirmationPopUp
{
    public virtual void CancelAction()
    {
        
    }
    public virtual void ConfirmAction()
    {
        
    }
}

public class BannerObjectController : PopUpObject, IBannerObjectController
{
    private Animator _mTextAnimator;
    private Animator _mBgAnimator;
    
    [SerializeField] private TMP_Text bannerText;
    private bool _mIsActive;

    public void SetBannerText(string newText)
    {
        bannerText.text = newText;
    }

    public void ToggleBannerGameObject(bool isActive)
    {
        gameObject.SetActive(isActive);        
    }

    public void ToggleBannerForSeconds(string newText, float seconds)
    {
        if (_mIsActive)
        {
            return;
        }
        _mIsActive = true;
        StartCoroutine(ToggleForSeconds(newText, seconds));
    }

    private IEnumerator ToggleForSeconds(string newText, float seconds)
    {
        _mBgAnimator = GetComponent<Animator>();
        _mTextAnimator = bannerText.GetComponent<Animator>();
        if (!_mBgAnimator || !_mTextAnimator)
        {
            yield break;
        }
        
        bannerText.text = newText;
        gameObject.SetActive(_mIsActive);
        _mBgAnimator.Play(AnimationHashData.BgFadeIn);
        _mTextAnimator.Play(AnimationHashData.TextFadeIn);
        
        
        yield return new WaitForSeconds(seconds);
        _mIsActive = false;

        _mBgAnimator.Play(AnimationHashData.BgFadeOut);
        _mTextAnimator.Play(AnimationHashData.TextFadeOut);
        var fadeTime = _mBgAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(fadeTime);
        bannerText.text = "";
        PopUpOperator.RemovePopUp(PopUpId);
    }
}