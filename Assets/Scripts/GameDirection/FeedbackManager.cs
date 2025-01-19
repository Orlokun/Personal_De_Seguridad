using System.Collections;
using System.Collections.Generic;
using DataUnits;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem.Interfaces;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

namespace GameDirection
{
    public interface IFeedbackManager
    {
        public void StartReadingFeedback(GeneralFeedbackId feedbackType);

        public void ActivatePhoneCallReceivedButton(ICallableSupplier caller);
        public void DeactivatePhoneCallReceivedButton();
    }

    public class FeedbackManager : MonoBehaviour, IInitialize, IFeedbackManager
    {
        private static FeedbackManager _mInstance;
        public static IFeedbackManager Instance => _mInstance;
        
        [SerializeField] private Image mFeedbackBaseObject;
        [SerializeField] private TMP_Text mFeedbackTextObject;
        
        private FeedbackTextsData _mFeedbackTextsData;
        private Dictionary<GeneralFeedbackId, GeneralFeedbackObject> _feedbackObjects = new Dictionary<GeneralFeedbackId, GeneralFeedbackObject>();
        private bool _mInitialized;
        public bool IsInitialized => _mInitialized;

        private bool _mIsActive;

        [SerializeField] private PhoneAnswerPopUp mPhoneCallObject;
        
        
        
        #region Init
        private void Awake()
        {
            if (mFeedbackBaseObject != null)
            {            
                mFeedbackBaseObject.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            if (_mInstance != null && _mInstance != this)
            {
                Destroy(this);
            }
            Initialize();
        }
        public void Initialize()
        {
            if (_mInitialized)
            {
                return;
            }
            var url = DataSheetUrls.FeedbacksGameDataUrl;
            StartCoroutine(GetFeedbacksData(url));
            _mInitialized = true;
            _mInstance = this;
        }
        private IEnumerator GetFeedbacksData(string url)
        {
            Debug.Log("[GetFeedbacksData] Creating Web request done");
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            Debug.Log("[GetFeedbacksData] Web request done");
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Feedback Data must be reachable. Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadFeedbackTextFromJson(sourceJson);
            }
        }
        private void LoadFeedbackTextFromJson(string sourceJson)
        {
            Debug.Log($"[FeedbackManager.LoadFeedbackTextFromJson] Begin request");
            _mFeedbackTextsData = JsonConvert.DeserializeObject<FeedbackTextsData>(sourceJson);
            Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mFeedbackTextsData == null}. {_mFeedbackTextsData}");
            if (_mFeedbackTextsData == null)
            {
                return;
            }
            _feedbackObjects = new Dictionary<GeneralFeedbackId, GeneralFeedbackObject>();
            for (int i = 1; i < _mFeedbackTextsData.values.Count; i++)
            {
                int.TryParse(_mFeedbackTextsData.values[i][0], out var feedbackId);
                var idCast = (GeneralFeedbackId) feedbackId;
                if (idCast == 0)
                {
                    continue;
                }
                var feedbackName = _mFeedbackTextsData.values[i][1];
                var feedbackText = _mFeedbackTextsData.values[i][2];
                int.TryParse(_mFeedbackTextsData.values[i][3], out var feedbackLength);
                var generalFeedbackObject = new GeneralFeedbackObject(idCast,feedbackName, feedbackText, feedbackLength);
                if (!_feedbackObjects.ContainsKey(idCast))
                {
                    _feedbackObjects.Add(generalFeedbackObject.FeedbackId, generalFeedbackObject);
                }
            }
        }
        #endregion

        /// <summary>
        /// API call to start reading
        /// </summary>
        /// <param name="feedbackType"></param>
        public void StartReadingFeedback(GeneralFeedbackId feedbackType)
        {
            if (!_mInitialized)
            {
                Initialize();
            }
            if (!_feedbackObjects.ContainsKey(feedbackType))
            {
                return;
            }
            ReadFeedbackForTime(_feedbackObjects[feedbackType].FeedbackText,
                _feedbackObjects[feedbackType].FeedbackReadTime);
        }

        public void ActivatePhoneCallReceivedButton(ICallableSupplier caller)
        {
            mPhoneCallObject.SetAnswerData(caller);
            mPhoneCallObject.gameObject.SetActive(true);
        }
        
        public void DeactivatePhoneCallReceivedButton()
        {
            mPhoneCallObject.CleanAnswerData();
            mPhoneCallObject.gameObject.SetActive(false);
        }
        
        private void ReadFeedbackForTime(string feedbackText, int timeLength)
        {
            if (_mIsActive)
            {
                return;
            }
            StartCoroutine(StartFeedbackDisplay(feedbackText, timeLength));
        }
        private IEnumerator StartFeedbackDisplay(string feedbackText, int time)
        {
            _mIsActive = true;
            mFeedbackTextObject.text = feedbackText;
            var bgAnim = mFeedbackBaseObject.GetComponent<Animator>();
            var textAnim = mFeedbackTextObject.GetComponent<Animator>();
            
            if (!bgAnim || !textAnim)
            {
                yield break;
            }
            mFeedbackBaseObject.gameObject.SetActive(_mIsActive);
            
            bgAnim.Play(AnimationHashData.BgFadeIn);
            textAnim.Play(AnimationHashData.TextFadeIn);
            var fadeClips = bgAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            var seconds = time + fadeClips;
            yield return new WaitForSeconds(seconds);
            
            mFeedbackTextObject.text = "";
            bgAnim.Play(AnimationHashData.BgFadeOut);
            textAnim.Play(AnimationHashData.TextFadeOut);
            fadeClips = bgAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            yield return new WaitForSeconds(fadeClips);
            _mIsActive = false;
            mFeedbackBaseObject.gameObject.SetActive(_mIsActive);
        }
    }
}