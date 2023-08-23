using DataUnits;
using DataUnits.ItemSources;
using DialogueSystem;
using DialogueSystem.Interfaces;
using TMPro;
using UnityEngine;

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
    private string _displayedString = "";
    private PhoneState _phoneState = PhoneState.Dialing;
    private IDialogueOperator _dialogueOperator;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _dialingSound;

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
        Debug.Log($"[PressCall] Not Implemented. Calling number: {_displayedString}");   
    }
    public void StartCallImmediately(ISupplierBaseObject callReceiver)
    {
        displayedText.text = callReceiver.SupplierName;
        ToggleDialingSound(true);
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
