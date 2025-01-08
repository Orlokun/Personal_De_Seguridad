using System.Collections.Generic;
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

    public interface INewsNarrativeDirector
    {
        public void LoadDayNews(DayBitId dayBitId);
        public List<INewsObject>GetDayNews(DayBitId dayBitId);
    }

    public interface INewsObject
    {
        public void Initialize(DayBitId dayBitId, int newsId, string newsTitle, string newsSubHeader ,string newsContent, string newsImage, int[] linkedNews);

        public DayBitId DayBitId { get; }
        public int NewsId { get; }
        public string NewsTitle { get; }
        public string NewsSubHead { get; }

        public string NewsContent { get; }
        public string NewsImageName { get; }
        [CanBeNull] public int[]LinkedNews { get; }
        
    }
}