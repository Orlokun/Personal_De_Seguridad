using System;
using System.Collections;
using DataUnits.GameCatalogues;
using DataUnits.JobSources;
using DialogueSystem;
using DialogueSystem.Interfaces;
using GameDirection;
using GamePlayManagement.BitDescriptions.Suppliers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Task = System.Threading.Tasks.Task;

public enum PhoneState
{
    HungUp,
    Calling,
    OnCall,
    ReceivingCall,
}

public interface IPhoneCallOperator
{
    void GoToCall(ISupplierBaseObject callReceiver);
    void FinishCallImmediately();
    void DialNumber(int number);
    void PlayAnswerSound();
    void StartCallFromSupplier(IDialogueObject receivedCallDialogue);
}

[RequireComponent(typeof(AudioSource))]
public class PhoneCallOperator : MonoBehaviour, IPhoneCallOperator
{
    private static PhoneCallOperator _mInstance;
    public static IPhoneCallOperator Instance => _mInstance;
    private Coroutine beingCalledRoutine;

    [SerializeField] private TMP_Text displayedText;
    [SerializeField] private AudioClip dialingSound;
    [SerializeField] private AudioClip invalidNumberSound;
    [SerializeField] private AudioClip numberSound;
    [SerializeField] private AudioClip pickUpSound;
    [SerializeField] private AudioClip hangUpSound;
    [SerializeField] private AudioClip receivePhoneCallSound;
    [SerializeField] private Image phoneBgColor;

    private string _displayedString = "";
    private PhoneState _phoneState = PhoneState.HungUp;
    private IDialogueObject waitingCall;
    
    private IDialogueOperator _dialogueOperator;
    private AudioSource _audioSource;

    private Coroutine currentRoutine;

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
        _audioSource.clip = numberSound;
        _audioSource.Play();
    }

    public void PlayAnswerSound()
    {
        _audioSource.Stop();
        _audioSource.clip = pickUpSound;
        _audioSource.Play();
    }

    public void StartCallFromSupplier(IDialogueObject receivedCallDialogue)
    {
        _phoneState = PhoneState.ReceivingCall;
        waitingCall = receivedCallDialogue;
        PlayReceiveCallSound();
        beingCalledRoutine = StartCoroutine(FinishCallIfNoAnswer());
    }

    private IEnumerator FinishCallIfNoAnswer()
    {   
        Random.InitState(DateTime.Now.Millisecond);
        var randomTime = Random.Range(4000, 7000);
        yield return new WaitForSeconds(randomTime);
        FinishCallImmediately();
    }

    private void AnswerCallFromSupplier()
    {
        StopCoroutine(beingCalledRoutine);
        _phoneState = PhoneState.Calling;
        _dialogueOperator.StartNewDialogue(waitingCall);
        waitingCall = null;
    }
    public void ClearNumbersOrHungUp()
    {
        _displayedString = "";
        displayedText.text = _displayedString;
        _phoneState = PhoneState.HungUp;
        _audioSource.Stop();
        StopCoroutine(currentRoutine);
    }
    public void PressCall()
    {
        if (_phoneState == PhoneState.ReceivingCall)
        {
            AnswerCallFromSupplier();
            return;
        }
        StartPhoneCall();
    }

    private void StartPhoneCall()
    {
        _phoneState = PhoneState.Calling;
        if (!IsValidCall(_displayedString))
        {
            PlayInvalidCall();
            return;
        }
        PlayDialSound();
        var toIntNumber = int.Parse(_displayedString);
        //Since item and job suppliers are separated. Both Tuples must be checked. Because I'm an idiot. 
        //TODO: Fix Separate Job / Item Supplier issue
        var itemSupplierExistence = BaseItemSuppliersCatalogue.Instance.ItemSupplierPhoneExists(toIntNumber);
        var jobSupplierExistence = BaseJobsCatalogue.Instance.JobSupplierPhoneNumberExists(toIntNumber);
        if (!itemSupplierExistence.Item1 && !jobSupplierExistence.Item1)
        {
            Debug.Log("[PressCall] Not a Supplier number. Display the number is inactive or choose Random Conversation");
            FinishCallImmediately();
            return;
        }
        
        if (itemSupplierExistence.Item1)
        {
            WaitAnswerFromItemSupplier(itemSupplierExistence.Item2);
            return;
        }
        if(jobSupplierExistence.Item1)
        {
            WaitAnswerFromJobSupplier(jobSupplierExistence.Item2);
            return;
        }
    }
    
    private void WaitAnswerFromItemSupplier(int calledSupplierId)
    {
        var itemSupplierObject = BaseItemSuppliersCatalogue.Instance.GetItemSupplierData((BitItemSupplier)calledSupplierId);
        var currentProfile = GameDirector.Instance.GetActiveGameProfile;
        itemSupplierObject.StartCalling(currentProfile.GeneralOmniCredits);
        GameDirector.Instance.GetDialogueOperator.OnDialogueCompleted += FinishCallImmediately;

    }
    private void WaitAnswerFromJobSupplier(int calledSupplierId)
    {
        var jobSupplierCallingData = BaseJobsCatalogue.Instance.GetJobSupplierObject((JobSupplierBitId)calledSupplierId);
        var currentProfile = GameDirector.Instance.GetActiveGameProfile;
        jobSupplierCallingData.StartCalling(currentProfile.GeneralOmniCredits);
        GameDirector.Instance.GetDialogueOperator.OnDialogueCompleted += FinishCallImmediately;
    }

    private bool IsValidCall(string dialedNumber)
    {
        bool isValid = dialedNumber.Length == 7;
        return isValid;
    }
    
    private void PlayDialSound()
    {
        _audioSource.clip = dialingSound;
        _audioSource.Play();
    }

    private void PlayReceiveCallSound()
    {
        _audioSource.clip = receivePhoneCallSound;
        _audioSource.Play();
    }
    
    private void PlayInvalidCall()
    {
        Debug.Log("[PlayInvalidCall] Empty Number: Play null sound and return");
        _phoneState = PhoneState.HungUp;
        _displayedString = "INVALID CALL";
        _audioSource.clip = invalidNumberSound;
        _audioSource.Play();
        var waitTime = _audioSource.clip.length;
        currentRoutine = StartCoroutine(WaitPhoneSoundAndHungUp(waitTime));
    }

    private IEnumerator WaitPhoneSoundAndHungUp(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ClearNumbersOrHungUp();
    }

    public void GoToCall(ISupplierBaseObject callReceiver)
    {
        _displayedString = callReceiver.StorePhoneNumber.ToString();
        displayedText.text = _displayedString;
        //Play Sound for a few seconds
        //Get Dialogue in shop
        //Each shop/object should be holding its dialogue. 
        //Dialogues should be divided in types: Inner/Interpersonal/MainDialogue
        //Each type should be divided as well
    }
    
    public void FinishCallImmediately()
    {
        Debug.Log($"[FinishCallImmediately] Not Implemented");   
        _phoneState = PhoneState.HungUp;
        displayedText.text = "";
        _audioSource.Stop();
        GameDirector.Instance.GetDialogueOperator.OnDialogueCompleted -= FinishCallImmediately;
    }
    
    private void ToggleDialingSound(bool isSoundActive)
    {
        Debug.Log($"[ToggleDialingSound] Not Implemented");   
    }
}
