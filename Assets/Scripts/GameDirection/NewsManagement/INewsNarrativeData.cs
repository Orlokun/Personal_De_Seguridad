using System.Collections.Generic;
using GameDirection.TimeOfDayManagement;
using JetBrains.Annotations;

namespace GameDirection.NewsManagement
{
    public interface INewsNarrativeData
    {
        void LoadDayNews(DayBitId dayBitId);
        bool IsDayLoaded(DayBitId dayBitId);
        [CanBeNull] List<INewsObject> GetDayNewsList(DayBitId dayBitId);
    }
}