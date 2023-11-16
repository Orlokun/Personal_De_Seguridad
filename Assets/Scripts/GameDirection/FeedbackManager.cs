using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public class FeedbackManager : MonoBehaviour, IInitialize, IFeedbackManager
    {
        private static FeedbackManager _mInstance;
        public static IFeedbackManager Instance => _mInstance;
        
        [SerializeField] private Image mFeedbackBaseObject;
        private TMP_Text mFeedbackBaseText;
        private FeedbackTextsData _mFeedbackTextsData;
        private Dictionary<GeneralFeedbackId, GeneralFeedbackObject> _feedbackObjects = new Dictionary<GeneralFeedbackId, GeneralFeedbackObject>();
        private bool isInitialized = false;
        public bool IsInitialized => isInitialized;

        private bool isDisplayingText = false;

        private void Awake()
        {
            if (mFeedbackBaseObject != null)
            {            
                mFeedbackBaseText = mFeedbackBaseObject.transform.GetChild(0).GetComponent<TMP_Text>();
                mFeedbackBaseObject.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            if (!isInitialized)
            {
                Initialize();
            }
        }
        
        public void Initialize()
        {
            if (isInitialized)
            {
                return;
            }
            isInitialized = true;
            _mInstance = this;

            var url = DataSheetUrls.FeedbacksGameDataUrl;
            StartCoroutine(GetFeedbacksData(url));
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

        public void StartReadingFeedback(GeneralFeedbackId feedbackType)
        {
            if (!isInitialized)
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

        private void ReadFeedbackForTime(string feedbackText, int timeLength)
        {
            if (isDisplayingText)
            {
                return;
            }
            StartCoroutine(StartFeedbackDisplay(feedbackText, timeLength));
        }

        private IEnumerator StartFeedbackDisplay(string feedbackText, int time)
        {
            isDisplayingText = true;
            mFeedbackBaseText.text = feedbackText;
            mFeedbackBaseObject.gameObject.SetActive(isDisplayingText);

            yield return new WaitForSeconds(time);
            
            isDisplayingText = false;
            mFeedbackBaseText.text = "";
            mFeedbackBaseObject.gameObject.SetActive(isDisplayingText);
        }
    }
}