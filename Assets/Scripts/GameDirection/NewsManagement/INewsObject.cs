using GameDirection.TimeOfDayManagement;
using JetBrains.Annotations;

namespace GameDirection.NewsManagement
{
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