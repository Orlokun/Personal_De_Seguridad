using System.Collections;
using DataUnits;
using DataUnits.GameCatalogues;
using DialogueSystem;
using DialogueSystem.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PhoneState
{
    Calling,
    Dialing,
    OnCall
}

public interface IPhoneCallOperator
{
    void StartCallImmediately(ISupplierBaseObject callReceiver);
    void FinishCallImmediately();
}

[RequireComponent(typeof(AudioSource))]
public class PhoneCallOperator : MonoBehaviour, IPhoneCallOperator
{
    private static PhoneCallOperator _mInstance;
    public static IPhoneCallOperator Instance => _mInstance;
    
    [SerializeField] private TMP_Text displayedText;
    [SerializeField] private AudioClip dialingSound;
    [SerializeField] private AudioClip invalidNumberSound;
    [SerializeField] private AudioClip numberSound;
    [SerializeField] private Image phoneBgColor;

    private string _displayedString = "";
    private PhoneState _phoneState = PhoneState.Dialing;
    private IDialogueOperator _dialogueOperator;
    private AudioSource _audioSource;

    #region Init
    private void Awake()
    {
        if (_mInstance != null)
        {
            Destroy(this);
        }
        _mInstance = this;
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _dialogueOperator = DialogueOperator.Instance;
    }
    #endregion

    public void DialNumber(int number)
    {
        if (_displayedString.Length > 7)
        {
            return;
        }
        var numberText = number.ToString();
        _displayedString += numberText;
        displayedText.text = _displayedString;
    }
    public void ClearNumbersOrHungUp()
    {
        _displayedString = "";
        displayedText.text = _displayedString;
        if (_phoneState == PhoneState.Calling || _phoneState == PhoneState.OnCall)
        {
            
        }
        _phoneState = PhoneState.Dialing;
    }
    public void PressCall()
    {
        _phoneState = PhoneState.Calling;
        if (_displayedString.Length != 7)
        {
            PlayInvalidCall();
            return;
        }
        if (!BaseItemSuppliersCatalogue.Instance.SupplierPhoneExists(_displayedString))
        {
            Debug.Log("[PressCall] Not a Supplier number. Display the number is inactive or choose Random Conversation");
            return;
        }

        PlayDialSound();
        
        var itemSupplier = BaseItemSuppliersCatalogue.Instance.GetItemSupplierDataFromPhone(_displayedString);
    }

    private void PlayDialSound()
    {
        _audioSource.clip = dialingSound;
        _audioSource.Play();
    }
    
    private IEnumerator WaitDialSound()
    {
        yield return new WaitForSeconds(1f);
    }
    private void PlayInvalidCall()
    {
        Debug.Log("[PlayInvalidCall] Empty Number: Play null sound and return");
        _phoneState = PhoneState.Dialing;
        ClearNumbersOrHungUp();
    }

    
    private IEnumerator WaitInvalidCallSound()
    {
        yield return new WaitForSeconds(1f);
    }
    
    public void StartCallImmediately(ISupplierBaseObject callReceiver)
    {
        displayedText.text = callReceiver.SupplierName;
        ToggleDialingSound(true);
        //Play Sound for a few seconds
        //Get Dialogue in shop
        //Each shop/object should be holding its dialogue. 
        //Dialogues should be divided in types: Inner/Interpersonal/MainDialogue
        //Each type should be divided as well
    }
    public void FinishCallImmediately()
    {
        Debug.Log($"[FinishCallImmediately] Not Implemented");   
    }
    private void ToggleDialingSound(bool isSoundActive)
    {
        Debug.Log($"[ToggleDialingSound] Not Implemented");   
    }
}
