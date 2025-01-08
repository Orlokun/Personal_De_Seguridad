using System;
using System.Collections;
using System.Collections.Generic;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem;
using GameDirection.TimeOfDayManagement;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace GameDirection.NewsManagement
{
    public interface INewsNarrativeData
    {
        void LoadDayNews(DayBitId dayBitId);
        bool IsDayLoaded(DayBitId dayBitId);
        [CanBeNull] List<INewsObject> GetDayNewsList(DayBitId dayBitId);
    }
    
    public class NewsNarrativeData : INewsNarrativeData
    {
        private readonly Dictionary<DayBitId, List<INewsObject>> _mLoadedNews;

        public NewsNarrativeData()
        {
            _mLoadedNews = new Dictionary<DayBitId, List<INewsObject>>();
        }

        public void LoadDayNews(DayBitId dayBitId)
        {
            Debug.Log($"START: Download news SOURCES DATA");
            if(_mLoadedNews.ContainsKey(dayBitId))
            {
                return;
            }
            GetDayNews(dayBitId);
        }
        private void GetDayNews(DayBitId dayBitId)
        {
            var url = DataSheetUrls.DayNewsDataUrl(dayBitId);
            GameDirector.Instance.ActCoroutine(GetDayNewsFromServer(url));
        }
        private IEnumerator GetDayNewsFromServer(string url)
        {
            //
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Jobs Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadTodaysNews(sourceJson);
            }
        }

        private void LoadTodaysNews(string sourceJson)
        {
            Debug.Log($"BaseJobsCatalogue.LoadJobSuppliersFromJson");
            var newsData = JsonConvert.DeserializeObject<DayNewsCatalogueData>(sourceJson);
            Debug.Log($"Finished parsing. Is _jobsCatalogueFromData null?: {newsData == null}");
            var dayNewsList = new List<INewsObject>();
            for (var i = 1; i < newsData.values.Count;i++)
            {
                var hasDayId = int.TryParse(newsData.values[i][0], out var dayBitId);
                var hasNewsId = int.TryParse(newsData.values[i][1], out var newsId);
                var hasNewsTitle = String.IsNullOrEmpty(newsData.values[i][2]);
                var hasNewsContent = String.IsNullOrEmpty(newsData.values[i][3]);

                if (!hasDayId || !hasNewsId || hasNewsTitle || hasNewsContent)
                {
                    Debug.LogWarning($"[LoadTodaysNews] News must have a Day and NewsId and Title and Content");
                    return;
                }
                
                var mDayBitId = (DayBitId) dayBitId;
                
                var newsTitle = newsData.values[i][2];
                
                var newsSubHead = newsData.values[i][3];
                var newsContent = newsData.values[i][4];
                
                var hasImage = String.IsNullOrEmpty(newsData.values[i][5]);
                var imageString = newsData.values[i][5];
                
                var hasLinkedNews = String.IsNullOrEmpty(newsData.values[i][6]);
                
                var linkedNews = hasLinkedNews ? newsData.values[i][6].Split(',') : null;
                int[] relatedNews = null;
                if(linkedNews!= null)
                {
                    relatedNews = DialogueProcessor.ProcessLinksStrings(linkedNews);
                }
                var newsObject = (INewsObject)ScriptableObject.CreateInstance<NewsObject>();
                newsObject.Initialize(mDayBitId, newsId, newsTitle,newsSubHead, newsContent, imageString, relatedNews);
                dayNewsList.Add(newsObject);
                //Add to the list of data. Only at final iteration.
                if(i == newsData.values.Count - 1)
                {
                    _mLoadedNews.Add(mDayBitId, dayNewsList);
                }
            }

        }


        public bool IsDayLoaded(DayBitId dayBitId)
        {
            return _mLoadedNews.ContainsKey(dayBitId);
        }

        public List<INewsObject> GetDayNewsList(DayBitId dayBitId)
        {
            if (IsDayLoaded(dayBitId))
            {
                return _mLoadedNews[dayBitId];
            }
            return null;
        }
    }
}