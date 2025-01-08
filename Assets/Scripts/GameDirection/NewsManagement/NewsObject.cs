using GameDirection.TimeOfDayManagement;
using JetBrains.Annotations;
using UnityEngine;

namespace GameDirection.NewsManagement
{
    public class NewsObject : ScriptableObject, INewsObject
    {
        public NewsObject()
        {

        }

        public void Initialize(DayBitId dayBitId, int newsId, string newsTitle, string newsSubHeader, string newsContent, string newsImage, int[] linkedNews)
        {
            if(IsCreated)
            {
                return;
            }
            _mDayBitId = dayBitId;
            _mNewsTitle = newsTitle;
            _mNewsSubHeader = newsSubHeader;
            _mNewsContent = newsContent;
            _mNewsImage = newsImage;
            _mLinkedNews = linkedNews;
            _mNewsId = newsId;
            _mCreated = true;
        }

        private bool _mCreated;
        private DayBitId _mDayBitId;
        private int _mNewsId;
        private string _mNewsTitle;
        private string _mNewsSubHeader;
        private string _mNewsContent;
        private string _mNewsImage;
        private int[] _mLinkedNews;

        public bool IsCreated => _mCreated;
        
        public DayBitId DayBitId => _mDayBitId;
        public int NewsId => _mNewsId;
        public string NewsTitle => _mNewsTitle;
        public string NewsSubHead => _mNewsSubHeader;
        public string NewsContent => _mNewsContent;
        public string NewsImageName => _mNewsImage;
        [CanBeNull] public int[] LinkedNews => _mLinkedNews;
    }
}