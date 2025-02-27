using System.Collections.Generic;
using DialogueSystem;
using GameDirection.TimeOfDayManagement;
using JetBrains.Annotations;

namespace GameDirection.NewsManagement
{
    public class NewsNarrativeDirector : INewsNarrativeDirector
    {
        private INewsNarrativeData _mNewsNarrativeData;

        public NewsNarrativeDirector()
        {
            _mNewsNarrativeData = new NewsNarrativeData();
        }
        
        public void LoadDayNews(DayBitId dayBitId)
        {
            _mNewsNarrativeData.LoadDayNews(dayBitId);
        }

        [CanBeNull]
        public List<INewsObject> GetDayNews(DayBitId dayBitId)
        {
            return _mNewsNarrativeData.GetDayNewsList(dayBitId);    
        }
    }
}