using System.Collections.Generic;
using GameDirection.TimeOfDayManagement;

namespace GameDirection.NewsManagement
{
    public interface INewsNarrativeDirector
    {
        public void LoadDayNews(DayBitId dayBitId);
        public List<INewsObject>GetDayNews(DayBitId dayBitId);
    }
}